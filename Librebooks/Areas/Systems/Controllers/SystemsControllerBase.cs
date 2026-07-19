using VectraBooks.Areas.Systems.Providers;
using VectraBooks.Data;
using Microsoft.AspNetCore.Mvc;

namespace VectraBooks.Areas.Systems.Controllers;

[Route("sys/[controller]")]
[ApiController]
public abstract class SystemsControllerBase (ISystemsManager sysManager, AppDbContext? context = null, ILogger<SystemsControllerBase>? logger = null) : ControllerBase
{
	protected readonly ISystemsManager Manager = sysManager;
	protected readonly ILogger<SystemsControllerBase>? Logger = logger;
	protected readonly AppDbContext? Context = context;

}
