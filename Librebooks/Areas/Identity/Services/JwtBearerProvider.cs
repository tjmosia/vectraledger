using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace VectraBooks.Areas.Identity.Services
{
	public class JwtBearerProvider
	{
		public static void Configure (WebApplicationBuilder builder)
		{
			builder.Services.Configure<JwtParams>(builder.Configuration.GetSection("JwtParams"));
			builder.Services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.Authority = "https://localhost:5262";
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateIssuerSigningKey = true,
						ValidateLifetime = true,
						ValidIssuer = builder.Configuration[JwtParams.GetKeyNames().Issuer],
						ValidAudience = builder.Configuration[JwtParams.GetKeyNames().Audience],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[JwtParams.GetKeyNames().SecretKey]!))
					};
					options.IncludeErrorDetails = true;
					options.SaveToken = true;
					options.Events = new JwtBearerEvents
					{
						OnMessageReceived = (context) =>
						{
							context.Request.Cookies.TryGetValue(JwtTokenKeys.AccessToken, out var token);

							if (token != null)
								context.Request.Headers.Authorization = $"Bearer {token}";

							return Task.CompletedTask;
						}
					};
				});

			//builder.Services.AddAuthorizationCore();
		}
	}
}
