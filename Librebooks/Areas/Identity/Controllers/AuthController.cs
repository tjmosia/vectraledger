using System.Security.Claims;
using VectraBooks.Areas.Companies.Services;
using VectraBooks.Areas.Identity.Data;
using VectraBooks.Areas.Identity.Models;
using VectraBooks.Areas.Identity.Models.Authentication.Models;
using VectraBooks.Areas.Identity.Services;
using VectraBooks.Core.Identity;
using VectraBooks.Core.Operations;
using VectraBooks.Data;
using VectraBooks.Extensions.Mvc;
using VectraBooks.Models.Entity.IdentitySpace;
using VectraBooks.Providers;
using VectraBooks.Providers.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace VectraBooks.Areas.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController (UserManagerExtension userManager,
	SignInManagerExtension signInManager,
	ILogger<SessionControllerBase> logger,
	VerificationStore verificationStore,
	IOptions<JwtParams> jwtParameters,
	IVerificationManager verificationManager,
	AppDbContext context,
	ICompanyStore companyStore)
	: SessionControllerBase(userManager, signInManager, logger)
{
	private readonly JwtParams jwtParameters = jwtParameters.Value;
	private readonly IVerificationManager verificationManager = verificationManager;
	private readonly VerificationStore verificationStore = verificationStore;
	private readonly AppDbContext context = context;
	private readonly ICompanyStore companyStore = companyStore;

	[HttpGet("")]
	public async Task<IActionResult> FindUserAsync ([FromQuery] UsernameModel.Request input)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var user = await userManager!.FindByEmailAsync(input.Email!);

		if (user == null)
			return NotFound();
		else
			return Ok(new FindUserDto(user));
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginAsync ([FromBody] LoginModel.Request input)
	{
		var validation = LoginModel.Validate(input);

		if (!validation.IsValid)
			return BadRequest(validation.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage)));

		var user = await userManager!.FindByEmailAsync(input.Email!);

		if (user == null)
			return Ok(TransactionResult.Failure(TransactionError.Create(nameof(input.Email),
					IdentityErrorDescriptions.InvalidEmail)));

		if (!user.EmailConfirmed)
			return Ok(TransactionResult.Failure(TransactionError.Create("Unverified", "true")));

		var result = await signInManager!.CheckPasswordSignInAsync(user, input.Password!, false);

		if (result.Succeeded)
		{
			user.DateLastLoggedIn = DateOnly.FromDateTime(DateTime.Now);
			await userManager.UpdateAsync(user);

			var nameClaim = (await userManager.GetClaimsAsync(user))
				.FirstOrDefault(p => p.Type == ClaimTypes.Name);

			var (Token, ExpiryDate) = signInManager.GenerateJsonWebToken(nameClaim!);
			SetAuthenticationCookie(HttpContext, Token, ExpiryDate);

			var claims = await userManager.GetClaimsAsync(user);
			var roles = await userManager.GetUserRolesAsync(user);

			return Ok(TransactionResult<object>
				.Success(new LoginDto(user, [.. roles], [.. claims], null)));
		}
		else
		{
			return Ok(TransactionResult.Failure(TransactionError.Create(nameof(input.Password),
					IdentityErrorDescriptions.PasswordMismatch)));
		}
	}

	[HttpPost("register")]
	public async Task<IActionResult> RegisterAsync ([FromBody] RegisterModel.Request input)
	{
		var modelState = RegisterModel.Validate(input);

		if (!modelState.IsValid)
			return BadRequest(modelState.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage)));

		var verifyEmail = await verificationManager.VerifyAsync(input.Email!, EmailVerificationTypes.Registration, input.Code!);

		if (!verifyEmail.Succeeded)
			return Ok(TransactionResult.Failure([.. verifyEmail.Errors.Select(p => TransactionError.Create(nameof(input.Code), p.Description))]));

		var user = new User
		{
			Email = input.Email!.ToLower(),
			UserName = input.Email.ToLower(),
			Name = input.Name,
			Surname = input.Surname,
			NormalizedEmail = userManager!.NormalizeEmail(input.Email),
			NormalizedUserName = userManager.NormalizeName(input.Email),
			DateLastLoggedIn = DateOnly.FromDateTime(DateTime.Now),
			DateRegistered = DateOnly.FromDateTime(DateTime.Now),
			EmailConfirmed = true
		};

		var createResult = await userManager.CreateAsync(user, input.Password!);

		if (createResult.Succeeded)
		{
			await verificationStore.DeleteAsync(verifyEmail.Model!);
			user = await userManager.FindByEmailAsync(user.Email);
			await userManager.AddClaimAsync(user!, new Claim(ClaimTypes.Name, user!.Email!));

			var nameClaim = (await userManager.GetClaimsAsync(user))
				.FirstOrDefault(p => p.Type == ClaimTypes.Name);

			var (Token, ExpiryDate) = signInManager!.GenerateJsonWebToken(nameClaim!);
			SetAuthenticationCookie(HttpContext, Token, ExpiryDate);

			return Ok(TransactionResult<object>
				.Success(new LoginDto(user,
				[.. await userManager.GetUserRolesAsync(user)],
				[.. await userManager.GetClaimsAsync(user)],
				(await companyStore.FindByUserIdAsync(user.Id)).FirstOrDefault())));
		}
		else
		{
			IList<TransactionError> errors = [];

			if (createResult.Errors.Any())
				foreach (var error in createResult.Errors)
				{
					if (error.Code == nameof(userManager.ErrorDescriber.DuplicateEmail)
						&& errors.FirstOrDefault(p => p.Code == nameof(input.Email)) == null)
						errors.Add(new TransactionError(nameof(input.Email), IdentityErrorDescriptions.DuplicateEmail));

					if (error.Code.Contains("Password")
						&& errors.FirstOrDefault(p => p.Code == nameof(input.Password)) == null)
						errors.Add(new TransactionError(nameof(input.Password), IdentityErrorDescriptions.PasswordWeak));
				}
			else
				errors.Add(new TransactionError("", "Unable to register user. Please try again."));

			return Ok(TransactionResult.Failure([.. errors]));
		}
	}

	[HttpPost("reset-password")]
	public async Task<IActionResult> ResetPasswordAsync ([FromBody] ResetPasswordModel.Request input)
	{
		var validation = ResetPasswordModel.Validate(input);

		if (!validation.IsValid)
			return BadRequest(validation.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage)));

		var verifyEmail = await verificationManager.VerifyAsync(input.Email!, EmailVerificationTypes.PasswordReset, input.Code!);

		if (!verifyEmail.Succeeded)
			return Ok(TransactionResult.Failure([.. verifyEmail.Errors.Select(p => TransactionError.Create(nameof(input.Code), p.Description))]));

		var user = await userManager!.FindByEmailAsync(input.Email!);

		if (user == null)
			return NotFound();

		var result = await userManager.CheckPasswordAsync(user, input.Password!);

		if (result)
			return Ok(TransactionResult.Failure(TransactionError.Create(nameof(input.Password), "You are already using this password.")));

		var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);
		var resetPasswordResult = await userManager.ResetPasswordAsync(user, resetPasswordToken, input.Password!);

		if (resetPasswordResult.Succeeded)
		{
			await verificationStore.DeleteAsync(verifyEmail.Model!);
			return Ok(TransactionResult.Success);
		}

		foreach (var error in resetPasswordResult.Errors)
			logger!.LogInformation("Model Error: Code: {code} - Message: {description}", error.Code, error.Description);

		return Ok(TransactionResult.Failure(TransactionError.Create(nameof(input.Password), "Password doesn't meet requirements.")));
	}

	[HttpPost("confirm-login"), Authorize]
	public async Task<IActionResult> ConfirmSignInAsync ()
	{
		if (!string.IsNullOrEmpty(User!.Identity!.Name))
		{
			var user = await userManager!.FindByEmailAsync(User!.Identity!.Name);

			if (user != null)
			{
				var claims = await userManager.GetClaimsAsync(user);
				var roles = await userManager.GetUserRolesAsync(user);
				var company = (await companyStore.FindByUserIdAsync(user.Id)).FirstOrDefault();
				return Ok(new LoginDto(user, [.. roles], [.. claims], company));
			}
			RemoveAuthenticatoinCookie(HttpContext);
		}

		return Unauthorized();
	}

	[HttpPost]
	[Authorize]
	[Route("logout")]
	public async Task<IActionResult> Logout ()
	{
		HttpContext.Request.Headers.Authorization = "";
		var accessTokenCookie = HttpContext.Request.Cookies[JwtTokenKeys.AccessToken];

		if (accessTokenCookie != null)
			HttpContext.Response.Cookies.Delete(JwtTokenKeys.AccessToken, GetCookieOptions());

		return Ok();
	}

	private static void SetAuthenticationCookie (HttpContext context, string token, DateTimeOffset expires)
	{
		context.Response.Cookies.Append(JwtTokenKeys.AccessToken, token, GetCookieOptions());
	}

	private static void RemoveAuthenticatoinCookie (HttpContext context)
	{
		context.Response.Cookies.Delete(JwtTokenKeys.AccessToken, GetCookieOptions());
	}

	private static CookieOptions GetCookieOptions ()
	{
		return new CookieOptions
		{
			HttpOnly = true,
			IsEssential = true,
			Secure = true,
			SameSite = SameSiteMode.None,
			Domain = "localhost",
			MaxAge = TimeSpan.FromDays(1),
		};
	}
}