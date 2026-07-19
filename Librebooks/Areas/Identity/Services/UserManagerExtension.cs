using System.Security.Claims;
using VectraBooks.Data;
using VectraBooks.Models.Entity.IdentitySpace;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace VectraBooks.Areas.Identity.Services;

public class UserManagerExtension : UserManager<User>
{
	private readonly IConfiguration Configuration;
	public readonly new IdentityErrorDescriberExtension ErrorDescriber;
	public readonly new ILogger<UserManagerExtension> Logger;
	public readonly AppDbContext Context;

	public UserManagerExtension (IUserStore<User> store,
		IOptions<IdentityOptions> optionsAccessor,
		IPasswordHasher<User> passwordHasher,
		IEnumerable<IUserValidator<User>> userValidators,
		IEnumerable<IPasswordValidator<User>> passwordValidators,
		ILookupNormalizer keyNormalizer,
		IdentityErrorDescriberExtension errors,
		IServiceProvider services,
		ILogger<UserManagerExtension> logger,
		IConfiguration configuration,
		AppDbContext context)
		: base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
	{
		Configuration = configuration;
		ErrorDescriber = errors;
		Logger = logger;
		Context = context;
	}

	public async Task<IdentityResult> GenerateRefreshTokenAsync (User user)
	{
		var salt = BCrypt.Net.BCrypt.GenerateSalt();
		var token = BCrypt.Net.BCrypt.HashPassword(user.UserName, salt);
		user.RefreshSecurityStamp = salt;
		user.RefreshLoginHash = token;
		return await UpdateAsync(user);
	}

	public async Task<IdentityResult> DeleteRefreshTokenAsync (User user)
	{
		user.RefreshSecurityStamp = null;
		user.RefreshLoginHash = null;
		return await UpdateAsync(user);
	}

	public async Task<bool> IsInRoleAsync (User user, string roleName, string? associatedValue)
	{
		var role = await Context.Roles.Where(p => p.NormalizedName == NormalizeName(roleName))
			.Include(p => p.Users!.Where(u => u.UserId == user.Id))
			.FirstOrDefaultAsync();

		if (role == null || role.Users!.Count == 0)
			return false;

		if (associatedValue != null && role.Users.First().AssociatedTo != associatedValue)
			return false;
		return true;

	}

	public async Task<bool> HasClaimASync (User user, Claim claim)
	{
		var claims = await GetClaimsAsync(user);

		if (claims.Any(c => NormalizeName(c.Type) == NormalizeName(claim.Type) && c.Value == claim.Value))
			return true;

		return false;
	}

	public async Task<IList<UserRole>> GetUserRolesAsync (User user)
	{
		return await Context.UserRoles.Where(p => p.UserId == user.Id)
				.Include(p => p.Role)
				.ToListAsync();
	}

	public async Task<IdentityResult> AddToRoleAsync (User user, string roleName, string associatedValue)
	{
		var role = await Context.Roles.Where(p => p.NormalizedName == NormalizeName(roleName))
				.FirstOrDefaultAsync();

		if (role == null)
			return IdentityResult.Failed(ErrorDescriber.InvalidRole("Role is invalid."));

		var existingUserRole = await Context.UserRoles.Where(p => p.UserId == user.Id && p.RoleId == role.Id)
			.FirstOrDefaultAsync();

		try
		{
			if (existingUserRole != null)
			{
				existingUserRole.AssociatedTo = associatedValue;
				Context.UserRoles.Update(existingUserRole);
			}
			else
			{
				var userRole = new UserRole
				{
					UserId = user.Id,
					RoleId = role.Id,
					AssociatedTo = associatedValue
				};

				await Context.UserRoles.AddAsync(userRole);
			}

			await Context.SaveChangesAsync();
			return IdentityResult.Success;
		}
		catch (Exception)
		{
			return IdentityResult.Failed();
		}
	}
}
