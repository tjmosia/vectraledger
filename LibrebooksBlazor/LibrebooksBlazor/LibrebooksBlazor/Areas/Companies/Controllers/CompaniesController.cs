using System.Linq;
using LibrebooksBlazor.Areas.Companies.Data;
using LibrebooksBlazor.Areas.Companies.Models;
using LibrebooksBlazor.Areas.Companies.Services;
using LibrebooksBlazor.Areas.Systems.Services;
using LibrebooksBlazor.Core.Operations;
using LibrebooksBlazor.Models.Entity.CompanySpace;
using LibrebooksBlazor.Models.Entity.IdentitySpace;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LibrebooksBlazor.Areas.Companies.Controllers;

/// <summary>
/// Converted controller attributes to endpoint-fluent mappings.
/// Register these endpoints in Program.cs by calling app.MapCompaniesEndpoints();
/// </summary>
public static class CompaniesController
{
	public static IEndpointConventionBuilder[] MapCompaniesEndpoints(this IEndpointRouteBuilder app)
	{
		var builders = new List<IEndpointConventionBuilder>();

		// GET /Companies/{id}
		var getBuilder = app.MapGet("/Companies/{id}", async (HttpContext http, int id, UserManager<User> userManager, ICompanyStore companyStore) =>
		{
			var userName = http.User?.Identity?.Name;
			if (string.IsNullOrEmpty(userName))
				return Results.Unauthorized();

			var user = await userManager.FindByNameAsync(userName);

			if (user == null)
				return Results.Unauthorized();

			var company = await companyStore.FindByIdAsync(id, user.Id);

			return company == null ? Results.NotFound() : Results.Ok(new CompanySummaryData(company));
		}).RequireAuthorization();

		builders.Add(getBuilder);

		// POST /Companies/create
		var postBuilder = app.MapPost("/Companies/create", async (HttpContext http, CompaniesModels.Request input, ISystemsManager sysManager, ICompanyStore companyStore) =>
		{
			var validationResult = CompaniesModels.Validate(input);

			if (!validationResult.IsValid)
				return Results.BadRequest(TransactionResult.Failure(validationResult.Errors.Select(p => TransactionError.Create(p.PropertyName, p.ErrorMessage))));

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
				Taxes = taxes.Select(p => new CompanyTax
				{
					TaxType = p,
				}).ToList()
			};

			if (!string.IsNullOrEmpty(input.Logo))
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
				return Results.StatusCode(StatusCodes.Status201Created);
			}
			else
			{
				return Results.Ok(TransactionResult.Failure(result.Errors.Select(p => TransactionError.Create(p.Code, p.Description))));
			}
		}).RequireAuthorization();

		builders.Add(postBuilder);

		return builders.ToArray();
	}
}
