using VectraBooks.Areas.Companies.Data;
using VectraBooks.Areas.Companies.Models;
using VectraBooks.Areas.Companies.Services;
using VectraBooks.Areas.Identity.Services;
using VectraBooks.Areas.Systems.Providers;
using VectraBooks.Core.Operations;
using VectraBooks.Extensions.Mvc;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace VectraBooks.Areas.Companies.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CompaniesController
	(ISystemsStore sysStore,
	UserManagerExtension userManager,
	ICompanyManager companyManager,
	ICompanyStore companyStore)
	: SessionControllerBase(userManager)
{
	private readonly ISystemsStore sysStore = sysStore;
	private readonly ICompanyManager manager = companyManager;

	[HttpGet]
	[Route("{id}")]
	public async Task<IActionResult> GetAsync ([FromRoute] int id)
	{
		var user = await userManager!.FindByNameAsync(User.Identity!.Name!);

		if (user == null)
			return Unauthorized();

		var company = await companyStore.FindByIdAsync(id, user.Id);

		return company == null ? NotFound() : Ok(new CompanySummaryData(company));
	}

	[HttpPost]
	[Route("create")]
	public async Task<IActionResult> CreateAsync ([FromBody] CompaniesModels.Request input)
	{
		var validationResult = CompaniesModels.Validate(input);

		if (!validationResult.IsValid)
			return BadRequest(TransactionResult.Failure([.. validationResult.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage))]));

        var user = await userManager!.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

		var country = await sysStore.FindCountryByIdAsync(input.CountryId);
		var currency = await sysStore.FindCurrencyByIdAsync(input.CurrencyId);

        if (country == null || currency == null)
			return BadRequest(country == null 
				? TransactionResult.Failure(new TransactionError(nameof(input.CountryId), "Invalid country.")) 
				: TransactionResult.Failure(new TransactionError(nameof(input.CurrencyId), "Invalid currency.")));

        var createResult = await manager.CreateAsync(new Company()
        {
            BusinessSectorId = input.BusinessSectorId,
            RegNumber = input.RegNumber,
            FaxNumber = input.FaxNumber,
            PhysicalAddress = input.PhysicalAddress,
            PostalAddress = input.PostalAddress,
            PhoneNumber = input.TelephoneNumber,
            EmailAddress = input.Email,
            VATNumber = input.VATNumber,
            TradingName = input.TradingName,
            LegalName = input.LegalName,
            Logo = input.Logo != null ? new CompanyLogo
            {
                Image = new CompanyImage
                {
                    PathName = input.Logo,
                    DateCreated = DateOnly.FromDateTime(DateTime.Now)
                }
            } : null
        }, user, country, currency);

		if (createResult.Succeeded)
            return Ok(new CompanySummaryData(createResult.Model!));

		return Ok(TransactionResult.Failure([.. createResult.Errors.Select(p => TransactionError.Create(p.Code, p.Description))]));
	}
}
