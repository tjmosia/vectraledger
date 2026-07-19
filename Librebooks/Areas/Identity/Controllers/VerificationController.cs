using VectraBooks.Areas.Identity.Models.Authentication.Models;
using VectraBooks.Areas.Identity.Services;
using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.GeneralSpace;
using VectraBooks.Providers.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VectraBooks.Areas.Identity.Controllers;

[Route("verifications")]
[ApiController]
[AllowAnonymous]
public class VerificationController (IVerificationManager verificationManager, UserManagerExtension userManager, ILogger<VerificationController> logger)
	: ControllerBase
{
	private readonly IVerificationManager verificationManager = verificationManager;
	private readonly UserManagerExtension userManager = userManager;
	private readonly ILogger<VerificationController> logger = logger;

	[HttpPost]
	[Route("request")]
	public async Task<IActionResult> SendAsync ([FromBody] VerificationModels.Create.Request model)
	{
		var validation = VerificationModels.Create.Validate(model);

		if (!validation.IsValid)
			return BadRequest(validation.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage)));

		var (Request, Code) = await verificationManager.AddAsync(new VerificationRequest(model.Email!, model.Reason!));

		if (Request != null)
		{
			logger.LogInformation("A verification request for {Email} with reason {Reason} has been created. Token = {token}", model.Email, model.Reason, Code);
			return Ok(TransactionResult.Success);
		}

		return Ok(TransactionResult.Failure(TransactionError.Create("", "Resend failed. Please try again later.")));
	}

	[HttpPost("verify")]
	public async Task<IActionResult> VerifyAsync ([FromBody] VerificationModels.Verify.Request model)
	{
		var validation = VerificationModels.Verify.Validate(model);

		if (!validation.IsValid)
			return BadRequest(validation.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage)));

		var result = await verificationManager.VerifyAsync(model.Email!, model.Reason!, model.Code!);

		if (!result.Succeeded)
			return Ok(TransactionResult.Failure([.. result.Errors]));

		return Ok(TransactionResult.Success);
	}
}
