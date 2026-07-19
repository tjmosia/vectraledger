using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;

namespace VectraBooks.Areas.Identity.Services
{
    public class AuthenticationServiceExtension : IAuthenticationService
    {
        public Task<AuthenticateResult> AuthenticateAsync (HttpContext context, string? scheme)
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync (HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync (HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync (HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync (HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }
}
