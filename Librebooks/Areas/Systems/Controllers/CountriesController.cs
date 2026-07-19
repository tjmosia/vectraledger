using VectraBooks.Areas.Systems.Data;
using VectraBooks.Areas.Systems.Models;
using VectraBooks.Areas.Systems.Providers;
using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.SystemSpace;

using Microsoft.AspNetCore.Mvc;

namespace VectraBooks.Areas.Systems.Controllers;

[Route("countries")]
[ApiController]
public class CountriesController (ISystemsManager systemManager, ILogger<CountriesController> logger)
	: SystemsControllerBase(systemManager, logger: logger)
{
	[HttpPost]
	public async Task<IActionResult> OnPostAsync ([FromBody] CountryModel.Create.Request model, CancellationToken cancellationToken = default)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var country = model.MapToCountry(new Country());

		country.NormalizeCode();

		var result = await Manager.AddCountryAsync(country, cancellationToken);

		return BadRequest(result);
	}

	[HttpPatch("{countryId}")]
	public async Task<IActionResult> OnPatchAsync ([FromBody] CountryModel.Update.Request model, [FromRoute] int countryId, CancellationToken cancellationToken = default)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var country = await Manager.FindCountryByIdAsync(countryId, cancellationToken);

		if (country == null)
			return NotFound();

		if (country.Name!.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase) &&
			country.DialingCode!.Equals(model.DialingCode, StringComparison.CurrentCultureIgnoreCase) &&
			country.Code!.Equals(model.Code, StringComparison.CurrentCultureIgnoreCase))
		{
			return Ok(TransactionResult<CountryCountryData>.Success(new CountryCountryData(country)));
		}
		var result = await Manager.UpdateCountryAsync(model.MapToCountry(country), cancellationToken);

		if (result.Succeeded)
			return Ok(TransactionResult<CountryCountryData>.Success(new CountryCountryData(result.Model!)));
		else
			return Ok(TransactionResult.Failure(result.Errors));
	}

	[HttpGet]
	[Route("{countryId}")]
	public async Task<IActionResult> OnGetAsync ([FromRoute] int countryId, CancellationToken cancellationToken)
	{
		var country = await Manager.FindCountryByIdAsync(countryId, cancellationToken);
		if (country == null)
			return NotFound();

		return Ok(new CountryCountryData(country));
	}

	[HttpGet]
	public async Task<IEnumerable<CountryCountryData>> OnGetAsync (CancellationToken cancellationToken = default)
	{
		var countries = await Manager.GetCountriesAsync(cancellationToken);
		return countries.Any() ? countries.Select(p => new CountryCountryData(p)).ToArray() : [];
	}

	[HttpDelete]
	public async Task<IActionResult> OnDeleteAsync ([FromBody] int[] countryIds, CancellationToken cancellationToken)
	{
		var countries = await Manager.GetCountriesByIdsAsync(countryIds, cancellationToken);

		if (!countries.Any())
			return Ok(TransactionResult.Success);

		var result = await Manager.DeleteCountryAsync([.. countries], cancellationToken);

		return Ok(result);
	}

	[HttpDelete]
	[Route("{id}")]
	public async Task<IActionResult> OnDeleteAsync ([FromRoute] int id)
	{
		var country = await Manager.FindCountryByIdAsync(id);

		if (country == null)
			return NotFound();

		return Ok(await Manager.DeleteCountryAsync([country!]));
	}
}
