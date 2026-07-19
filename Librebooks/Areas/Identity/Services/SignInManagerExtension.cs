using System.Security.Claims;
using System.Text;

using VectraBooks.Models.Entity.IdentitySpace;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace VectraBooks.Areas.Identity.Services
{
	public class SignInManagerExtension : SignInManager<User>
	{
		public readonly IConfiguration Configuration;
		private readonly JwtParams jwtParams;

		public SignInManagerExtension (UserManager<User> userManager,
			IHttpContextAccessor contextAccessor,
			IUserClaimsPrincipalFactory<User> claimsFactory,
			IOptions<IdentityOptions> optionsAccessor,
			ILogger<SignInManager<User>> logger,
			IAuthenticationSchemeProvider schemes,
			IUserConfirmation<User> confirmation,
			IConfiguration configuration,
			IOptions<JwtParams> jwtParameters)
			: base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
		{
			Configuration = configuration;
			jwtParams = jwtParameters.Value;
		}

		public (string Token, DateTime ExpiryDateTime) GenerateJsonWebToken (params Claim[] userClaims)
		{
			if (userClaims.Length == 0)
				throw new ArgumentException("Exception occured at GenerateJsonWebToken. \n Cause: No claims were provided to create the token.", new ArgumentNullException());

			var expiryDate = DateTime.Now.AddHours(jwtParams.ExpiryTimeInMinutes);

			return (
				Token: new JsonWebTokenHandler()
					.CreateToken(
						new SecurityTokenDescriptor
						{
							Issuer = jwtParams.Issuer,
							Audience = jwtParams.Audience,
							Subject = new ClaimsIdentity(userClaims),
							Expires = expiryDate,
							SigningCredentials = new SigningCredentials(
								key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParams.SecretKey!)),
								algorithm: SecurityAlgorithms.HmacSha256
							)
						}),
				ExpiryDateTime: expiryDate
			);
		}

		public object GenerateUserSessionDTO (User user)
			=> new
			{
				Email = user.Email!,
				FirstName = user.Name!,
				LastName = user.Surname!,
				Photo = user.Photo
			};
	}
}
