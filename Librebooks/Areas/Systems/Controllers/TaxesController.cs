using VectraBooks.Areas.Systems.Data;
using VectraBooks.Areas.Systems.Models;
using VectraBooks.Areas.Systems.Providers;
using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.SystemSpace;
using Microsoft.AspNetCore.Mvc;

namespace VectraBooks.Areas.Systems.Controllers;

[ApiController]
public class TaxesController (SystemsStore systemsStore)
	: SystemsControllerBase(systemManager)
{
	private readonly SystemsStore store = systemsStore;
	[Route(""), HttpGet]
	public async Task<IList<TaxData>> GetAsync ()
	{
		return [.. (await store.GetTaxesAsync())
			.Select(p => new TaxData(p))];
	}

	[Route("{id}"), HttpGet]
	public async Task<IActionResult> GetAsync (int id)
	{
		var tax = await store.FindTaxByIdAsync(id);
		if (tax == null)
			return NotFound();
		return Ok(new TaxData(tax));
	}

	[HttpPost]
	public async Task<IActionResult> AddAsync ([FromBody] TaxAddModel.Request model)
	{
		var modelState = TaxAddModel.Validate(model);

		if (!modelState.IsValid)
			return BadRequest(TransactionResult.Failure([.. modelState.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage))]));

		var tax = new Tax
		{
			Rate = model.Rate,
			Name = model.Name,
			System = model.System,
			Type = model.Type
		};

		var result = await store.AddTaxAsync(tax);

		if (result.Succeeded)
			return Ok(TransactionResult<TaxData>.Success(new TaxData(result.Model!)));

		return Ok(result);
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> UpdateAsync ([FromBody] TaxUpdateModel.Request model, int id)
	{
		var modelState = TaxAddModel.Validate(model);

		if (!modelState.IsValid)
			return BadRequest(TransactionResult.Failure([.. modelState.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage))]));

		var tax = await store.FindTaxByIdAsync(id);

		if (tax == null)
			return NotFound();

		if (tax.Rate == model.Rate && tax.Name == model.Name && tax.Type == model.Type && tax.System == model.System)
			return Ok(TransactionResult<TaxData>.Success(new TaxData(tax)));

		tax.Type = model.Type;
		tax.Rate = model.Rate;
		tax.System = model.System;
		tax.Name = model.Name;

		var result = await store.UpdateTaxAsync(tax);

		if (result.Succeeded)
			return Ok(TransactionResult<TaxData>.Success(new TaxData(result.Model!)));

		return Ok(result);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAsync (int id)
	{
		var tax = await store.FindTaxByIdAsync(id);

		if (tax == null)
			return NotFound();

		var result = await store.DeleteTaxesAsync([tax]);
		return Ok(result);
	}

	[HttpDelete]
	public async Task<IActionResult> DeleteAsync ([FromBody] int[] ids)
	{
		var taxes = await store.GetTaxesAsync();
		var taxesToDelete = new List<Tax>();

		foreach (var id in ids)
			foreach (var tax in taxes)
				if (id == tax.Id)
					taxesToDelete.Add(tax);

		if (taxesToDelete.Count == 0)
			return Ok(TransactionResult.Success);

		var result = await store.DeleteTaxesAsync([.. taxesToDelete]);
		return Ok(result);
	}
}
