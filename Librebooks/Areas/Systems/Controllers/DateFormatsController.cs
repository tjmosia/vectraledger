using VectraBooks.Areas.Systems.Data;
using VectraBooks.Areas.Systems.Models;
using VectraBooks.Areas.Systems.Providers;
using VectraBooks.Core.Operations;
using Microsoft.AspNetCore.Mvc;

namespace VectraBooks.Areas.Systems.Controllers;

[ApiController]
public class DateFormatsController (ISystemsStore store) : ControllerBase
{
	private readonly ISystemsStore store = store;

	[HttpGet]
	public async Task<IList<DateFormatData>> OnGetAsync ()
	{
		return [.. (await store.FindDateFormatsAsync()).Select(p => new DateFormatData(p))];
	}

	[HttpPost]
	public async Task<IActionResult> OnPostAsync ([FromBody] DateFormatsAddModels.Request model)
	{
		var modelState = DateFormatsAddModels.Validate(model);

		if (!modelState.IsValid)
			return BadRequest(TransactionResult.Failure([..modelState.Errors
				.Select( p=> TransactionError.Create(p.PropertyName, p.ErrorMessage))]));

		var result = await store.CreateDateFormatAsync(new()
		{
			Format = model.Format,
		});

		return Ok(result);
	}

	[HttpPost("{id}")]
	public async Task<IActionResult> OnPatchAsync (int id, [FromBody] DateFormatsAddModels.Request model, CancellationToken cancellationToken)
	{
		var modelState = DateFormatsAddModels.Validate(model);

		if (!modelState.IsValid)
			return BadRequest(TransactionResult.Failure([..modelState.Errors
				.Select( p=> TransactionError.Create(p.PropertyName, p.ErrorMessage))]));

		var format = await store.FindDateFormatByIdAsync(id, cancellationToken);
		if (format == null)
			return NotFound();
		format.Format = model.Format;

		return Ok(await store.UpdateDateFormatAsync(format, cancellationToken));
	}
}
