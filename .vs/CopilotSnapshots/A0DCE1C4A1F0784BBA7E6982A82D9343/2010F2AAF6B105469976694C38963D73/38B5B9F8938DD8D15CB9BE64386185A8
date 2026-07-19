using LibrebooksBlazor.Areas.Companies.Data;
using LibrebooksBlazor.Areas.Companies.Models;
using LibrebooksBlazor.Areas.Companies.Services;
using LibrebooksBlazor.Areas.Systems.Services;
using LibrebooksBlazor.Core.Operations;
using LibrebooksBlazor.Extensions.Mvc;
using LibrebooksBlazor.Models.Entity.CompanySpace;
using LibrebooksBlazor.Models.Entity.IdentitySpace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibrebooksBlazor.Areas.Companies.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CompaniesController
	(ISystemsManager sysManager,
	ICompanyManager companyManager,
	UserManager<User> userManager,
	ICompanyStore companyStore)
	: SessionControllerBase(userManager)
{
	private readonly ISystemsManager sysManager = sysManager;
	private readonly ICompanyManager companyManager = companyManager;
	private readonly ICompanyStore companyStore = companyStore;

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

		var taxes = await sysManager.GetTaxesAsync();

		var company = new Company()
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

			RegionalSetup = new()
			{
				DateFormatId = input.DateFormatId,
				DecimalMark = ".",
				RoundToNearest = 2,
				ThousandsSeperator = " "
			},
			Taxes = [..taxes.Select(p => new CompanyTax
			{
				TaxType = p,
			})]
		};

		if (input.Logo != null)
		{
			company.Logo = new CompanyLogo
			{
				Image = new CompanyImage
				{
					PathName = input.Logo
				}
			};
		}

		var result = await companyStore.CreateAsync(company);

		if (result.Succeeded)
		{
			return Created();
		}
		else
		{
			return Ok(TransactionResult.Failure([.. result.Errors.Select(p => TransactionError.Create(p.Code, p.Description))]));
		}
	}
}
