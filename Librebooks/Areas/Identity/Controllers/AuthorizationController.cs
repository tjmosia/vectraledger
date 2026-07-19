using System.Security.Claims;
using VectraBooks.Areas.Identity.Models.Authorization;
using VectraBooks.Areas.Identity.Services;
using VectraBooks.Extensions.Mvc;
using VectraBooks.Models.Entity.IdentitySpace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VectraBooks.Areas.Identity.Controllers;

[Route("authorize/[controller]")]
[Authorize, ApiController]
public class AuthorizationController (UserManagerExtension userManager) :
	SessionControllerBase(userManager)
{
	[HttpPost("role")]
	public async Task<IActionResult> AuthorizeByRoleASync ([FromBody] AuthorizationModels.RoleRequest input)
	{
		if (input.Role != null)
		{
			var user = await GetCurrentUserAsync(User);

			if (user != null && await userManager!.IsInRoleAsync(user, input.Role, input.Value))
				return Ok();
		}

		return Forbid();
	}

	[HttpPost("claim")]
	public async Task<IActionResult> AuthorizeByClaimASync ([FromBody] AuthorizationModels.ClaimRequest input)
	{
		var user = await GetCurrentUserAsync(User);

		if (input.ClaimType != null && input.Value != null)
		{
			if (user != null)
			{
				var hasClaim = await userManager!.HasClaimASync(user, new Claim(input.ClaimType, input.Value));

				if (hasClaim)
					return Ok();
			}
		}
		return Forbid();
	}

	private async Task<User?> GetCurrentUserAsync (ClaimsPrincipal principal)
	{
		return await userManager!.GetUserAsync(principal);
	}
}


