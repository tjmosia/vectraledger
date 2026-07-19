using System.Security.Claims;
using VectraBooks.Areas.Identity.Models.Account.Models;
using VectraBooks.Areas.Identity.Services;
using VectraBooks.Core.Operations;
using VectraBooks.Extensions.Mvc;
using VectraBooks.Models.Entity.IdentitySpace;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace VectraBooks.Areas.Identity.Controllers;

[Authorize]
[Route("account")]
[ApiController]
public class AccountController (
		UserManagerExtension userManager,
		ILogger<SessionControllerBase> logger,
		SignInManagerExtension signInManager,
		IOptions<JwtParams> jwtParameters

	) : SessionControllerBase(
		userManager: userManager,
		signInManager: signInManager,
		logger: logger
	)
{
	private readonly JwtParams jwtParameters = jwtParameters.Value;

	[HttpPost]
	[Route("claims")]
	public async Task<IActionResult> GetClaimsAsync ()
	{
		logger!.LogInformation("{token}", HttpContext.Request.Headers.Authorization.FirstOrDefault());
		var username = User.Identity!.Name;
		if (username == null)
			return Forbid();

		var user = await userManager!.FindByNameAsync(username);

		if (user == null)
		{
			RemoveAuthenticationCookie(HttpContext);
			return Unauthorized();
		}

		return Ok(await userManager.GetClaimsAsync(user));
	}

	[HttpPost]
	[Route("roles")]
	public async Task<IActionResult> GetRolesAsync ()
	{
		var user = await userManager!.FindByNameAsync(User.Identity!.Name!);

		if (user == null)
		{
			RemoveAuthenticationCookie(HttpContext);
			return Unauthorized();
		}

		return Ok(await userManager.GetRolesAsync(user));
	}

	[HttpPost]
	[Route("profile")]
	public async Task<IActionResult> GetProfileAsync ()
	{
		var user = await userManager!.FindByNameAsync(User.Identity!.Name!);

		if (user == null)
		{
			RemoveAuthenticationCookie(HttpContext);
			return Unauthorized();
		}

		return Ok(new
		{
			user.Name,
			user.Surname,
			user.Email,
			user.PhoneNumber,
			user.DateRegistered,
			user.Photo
		});
	}

	[HttpPost]
	[Route("personal-info/edit")]
	public async Task<IActionResult> UpdateUserPersonalInfoAsync ([FromBody] UpdatePersonalInfoModel.Request input)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var user = await userManager!.FindByNameAsync(User.Identity!.Name!);

		if (user == null)
		{
			RemoveAuthenticationCookie(HttpContext);
			return Unauthorized();
		}

		try
		{
			user.Name = input.Name;
			user.Surname = input.Surname;

			await userManager.UpdateAsync(user);

			return Ok(TransactionResult<object>.Success(GenerateUserSessionDto(user)));
		}
		catch (Exception ex)
		{
			logger!.LogError("{stackTrace}", ex.StackTrace);
			return BadRequest(ModelState);
		}
	}

	[HttpPost]
	[Route("logout")]
	public IActionResult Logout ()
	{
		RemoveAuthenticationCookie(HttpContext);
		return Ok();
	}

	[HttpPost]
	[Route("change-email")]
	public async Task<IActionResult> ChangeEmailAsync ([FromBody] ChangeEmailModel.Request model)
	{
		var modelState = ChangeEmailModel.Validate(model);

		if (!modelState.IsValid)
			return BadRequest(modelState.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage)));

		if (User.Identity!.Name!.Equals(model.Email!, StringComparison.OrdinalIgnoreCase))
			return Ok(TransactionResult.Failure(
				TransactionError.Create(nameof(model.Email), "You're already using this email.")));

		var _user = await userManager!.FindByEmailAsync(model.Email!);
		if (_user != null)
			return Ok(TransactionResult.Failure(
				TransactionError.Create(nameof(model.Email), "This email is already in use.")));


		var user = await userManager!.FindByEmailAsync(User.Identity!.Name!);

		if (user == null)
		{
			RemoveAuthenticationCookie(HttpContext);
			return Unauthorized();
		}

		try
		{
			user.Email = model.Email!.ToLower();
			user.EmailConfirmed = true;
			user.UserName = model.Email!.ToLower();
			user.NormalizedEmail = userManager.NormalizeEmail(user.UserName);
			user.NormalizedUserName = userManager.NormalizeName(user.UserName);
			var result = await userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				await userManager.SetUserNameAsync(user, model.Email);
				await userManager.UpdateNormalizedUserNameAsync(user);
				var claims = await userManager.GetClaimsAsync(user);

				await userManager.ReplaceClaimAsync(user,
					claims.Where(p => p.Type == ClaimTypes.Name).First(),
					new Claim(ClaimTypes.Name, user.Email));

				var newClaims = await userManager.GetClaimsAsync(user);

				(string Token, DateTime ExpiryDateTime) = signInManager!.GenerateJsonWebToken(newClaims.Where(p => p.Type == ClaimTypes.Name).First());
				RemoveAuthenticationCookie(HttpContext);
				SetAuthenticationCookie(HttpContext, Token, ExpiryDateTime);

				return Ok(TransactionResult.Success);
			}

			return Ok(TransactionResult.Failure(
				TransactionError.Create(nameof(model.Email),
				result.Errors.FirstOrDefault()?.Description ?? "Unable to change email. Please try again.")
			));
		}
		catch (Exception ex)
		{
			ModelState.AddModelError("Email", "Server error. Please try again.");
			return BadRequest(ex);
		}
	}

	[HttpPost]
	[Route("change-password")]
	public async Task<IActionResult> ChangePasswordAsync ([FromBody] ChangePasswordModel.Request input)
	{
		var modelState = ChangePasswordModel.Validate(input);
		if (!modelState.IsValid)
			return BadRequest(modelState.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage)));

		var user = await userManager!.FindByEmailAsync(User.Identity!.Name!);

		if (user == null)
		{
			RemoveAuthenticationCookie(HttpContext);
			return Unauthorized();
		}

		if (input.Password == input.OldPassword)
			return Ok(TransactionResult.Failure(
				TransactionError.Create(nameof(input.Password), "Use a different password.")));

		if (!await userManager.CheckPasswordAsync(user, input.OldPassword!))
			return Ok(TransactionResult.Failure(
				TransactionError.Create(nameof(input.OldPassword), "Incorrect password.")));

		var result = await userManager.ChangePasswordAsync(user, input.OldPassword!, input.Password!);

		TransactionResult transactionResult = TransactionResult.Success;

		if (transactionResult.Succeeded)
			return Ok(transactionResult);

		TransactionError? resultError = TransactionError.Create(nameof(input.Password), "Password doesn't meet requirements.");

		foreach (var error in result.Errors)
		{
			if (error.Code == nameof(userManager.ErrorDescriber.PasswordMismatch))
			{
				resultError = TransactionError.Create(nameof(input.OldPassword), "Incorrect password.");
				break;
			}
		}

		return Ok(TransactionResult.Failure(resultError));
	}

	private static object GenerateUserSessionDto (User user)
	{
		return new
		{
			user.Name,
			user.Surname,
			user.Email,
			user.Photo
		};
	}

	private void SetAuthenticationCookie (HttpContext context, string token, DateTimeOffset expires)
	{
		context.Response.Cookies.Append(JwtTokenKeys.AccessToken, token, new CookieOptions
		{
			HttpOnly = true,
			Expires = expires,
			IsEssential = true,
			SameSite = SameSiteMode.Strict,
			Domain = "localhost",
			Secure = true, // Change to True in Production
			MaxAge = TimeSpan.FromMinutes(jwtParameters.ExpiryTimeInMinutes)
		});
	}

	private void RemoveAuthenticationCookie (HttpContext context)
	{
		HttpContext.Response.Cookies.Delete(JwtTokenKeys.AccessToken);
	}
}