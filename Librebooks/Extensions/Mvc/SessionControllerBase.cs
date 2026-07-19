using VectraBooks.Areas.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace VectraBooks.Extensions.Mvc
{
	[Route("api/[controller]")]
	[ApiController]
	public abstract class SessionControllerBase
		: ControllerBase
	{
		protected readonly UserManagerExtension? userManager;
		protected readonly SignInManagerExtension? signInManager;
		protected readonly ILogger<SessionControllerBase>? logger;

		public SessionControllerBase (
			UserManagerExtension? userManager = null,
			SignInManagerExtension? signInManager = null,
			ILogger<SessionControllerBase>? logger = null)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.logger = logger;
		}
	}
}
