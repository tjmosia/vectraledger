using VectraBooks.Core.Constants;
using VectraBooks.Core.EFCore;
using VectraBooks.Core.Operations;
using VectraBooks.Core.Util;
using VectraBooks.Data;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.SystemSpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Areas.Systems.Providers;

public class SystemsStore (AppDbContext context, ILogger<SystemsStore> logger) : DbStoreBase(context), ISystemsStore
{
	private readonly ILogger<SystemsStore> logger = logger;

	/*****************************************************************
	 * COMPNY NUMBER OPERATIONS
	 *****************************************************************/
	public async Task<CompanySetup?> GetCurrentCompanySetupAsync (CancellationToken cancellationToken = default)
		=> await context!.CompanySetups!.FirstOrDefaultAsync(cancellationToken);

	public async Task<TransactionResult<CompanySetup>> CreateCompanySetupAsync (CompanySetup setup, CancellationToken cancellationToken = default)
	{
		try
		{
			var add = await context!.CompanySetups!.AddAsync(setup, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<CompanySetup>.Success(add.Entity);
		}
		catch (Exception)
		{
			return TransactionResult<CompanySetup>.Failure(GeneralError);
		}
	}

	public async Task<TransactionResult<CompanySetup>> UpdateCompanySetupAsync (CompanySetup setup, CancellationToken cancellationToken = default)
	{
		try
		{
			setup.RefreshConcurrencyToken();
			var result = context!.CompanySetups!.Update(setup);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<CompanySetup>.Success(result.Entity);
		}
		catch (Exception)
		{
			return TransactionResult<CompanySetup>.Failure(GeneralError);
		}
	}

	/*****************************************************************
	 * TAX OPERATIONS
	 *****************************************************************/
	public async Task<TransactionResult<Tax>> CreateTaxAsync (Tax vat, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.Taxes!.AddAsync(vat, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<Tax>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			return TransactionResult<Tax>.Failure(() =>
			{
				if (IsUniqueConstaint(ex) && ex.InnerException!.Message.Contains("Type"))
					return new TransactionError(nameof(Tax.Type), "Tax type already exists.");
				return GeneralError;
			});
		}
	}

	public async Task<TransactionResult<Tax>> UpdateTaxAsync (Tax vat, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context!.Taxes!.Update(vat);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<Tax>.Success(result.Entity);
		}
		catch (Exception)
		{
			return TransactionResult<Tax>.Failure(GeneralError);
		}
	}

	public async Task<Tax?> FindTaxByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.Taxes!.FindAsync([id], cancellationToken);

	public async Task<TransactionResult> DeleteTaxesAsync (Tax[] taxes, CancellationToken cancellationToken = default)
	{
		try
		{
			context!.Taxes!.RemoveRange(taxes);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (IsForeignKeyViolation(ex))
				errors.Add(new(description: taxes.Length > 1 ? "One or more taxes are currently in use." : "Tax is currently in use."));
			if (errors.Any())
				errors.Add(GeneralError);
			return TransactionResult.Failure([.. errors]);
		}
	}

	public async Task<IList<Tax>> GetTaxesAsync (CancellationToken cancellationToken = default)
		=> await context!.Taxes!.Where(p => p.System).ToListAsync(cancellationToken);


	/*****************************************************************
	 * COUNTRY OPERATIONS
	 *****************************************************************/

	private const string UNIQUE_COUNTRY_NAME_ERROR = "Country name already exists.";
	private const string UNIQUE_COUNTRY_CODE_ERROR = "Country code already exists.";
	public async Task<Country?> FindCountryByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.Countries!.FindAsync([id], cancellationToken);

	public async Task<IList<Country>> GetCountriesAsync (CancellationToken cancellationToken = default)
		=> await context!.Countries!.ToListAsync(cancellationToken);

	public async Task<IList<Country>> FindCountryByIdsAsync (int[] ids, CancellationToken cancellationToken = default)
		=> await context!.Countries!.Where(p => ids.Contains(p.Id)).ToListAsync(cancellationToken);

	public async Task<TransactionResult<Country>> CreateAsync (Country country, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.Countries!.AddAsync(country, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<Country>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			return TransactionResult<Country>.Failure(() =>
			{
				if (IsUniqueConstaint(ex))
				{
					if (ex.InnerException!.Message.Contains("Name", StringComparison.InvariantCultureIgnoreCase))
						return new TransactionError(nameof(Country.Name), UNIQUE_COUNTRY_NAME_ERROR);
					else if (ex.InnerException.Message.Contains("Code", StringComparison.InvariantCultureIgnoreCase))
						return new TransactionError(nameof(Country.Code), UNIQUE_COUNTRY_CODE_ERROR);
				}
				return GeneralError;
			});
		}
	}

	public async Task<TransactionResult<Country>> UpdateCountryAsync (Country country, CancellationToken cancellationToken = default)
	{
		try
		{
			country.RowVersion = GenerateRowVersion();
			var result = context!.Countries!.Update(country);
			await context.SaveChangesAsync(cancellationToken);

			return TransactionResult<Country>.Success(result.Entity);
		}
		catch (DbUpdateException ex)
		{
			return TransactionResult<Country>.Failure(() =>
			{
				if (IsUniqueConstaint(ex))
				{
					if (ex.InnerException!.Message.Contains("Name", StringComparison.InvariantCultureIgnoreCase))
						return new TransactionError(nameof(Country.Name), UNIQUE_COUNTRY_NAME_ERROR);
					else if (ex.InnerException.Message.Contains("Code", StringComparison.InvariantCultureIgnoreCase))
						return new TransactionError(nameof(Country.Code), UNIQUE_COUNTRY_CODE_ERROR);
				}

				return GeneralError;

			});
		}
	}

	public async Task<TransactionResult> DeleteCountriesAsync (params Country[] countries)
	{
		try
		{
			context!.Countries!.RemoveRange(countries);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (DbUpdateException ex)
		{
			return TransactionResult.Failure(() =>
			{
				if (IsForeignKeyViolation(ex))
					return new TransactionError(description: countries.Length > 1 ? "One or more countries are currently in use." : "Country is currently in use.");

				return GeneralError;

			});
		}
	}


	/*****************************************************************
	 * CURRENCY OPERATIONS
	 *****************************************************************/

	private const string UNIQUE_CURRENCY_NAME_ERROR = "Currency name already exists";
	private const string UNIQUE_CURRENCY_CODE_ERROR = "Currency code already exists.";

	public async Task<IList<Currency>> GetCurrenciesAsync (CancellationToken cancellationToken = default)
		=> await context!.Currencies!.ToListAsync(cancellationToken);

	public async Task<Currency?> FindCurrencyByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.Currencies!.FindAsync([id], cancellationToken);

	public async Task<TransactionResult<Currency>> CreateCurrencyAsync (Currency currency, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.Currencies!.AddAsync(currency, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);

			return TransactionResult<Currency>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("Error occured while creating currency with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
			{
				if (ex.InnerException!.Message.Contains("Name", StringComparison.InvariantCultureIgnoreCase))
					errors.Add(new(nameof(Currency.Name), UNIQUE_CURRENCY_NAME_ERROR));
				else if (ex.InnerException.Message.Contains("Code", StringComparison.InvariantCultureIgnoreCase))
					errors.Add(new(nameof(Currency.Code), UNIQUE_CURRENCY_CODE_ERROR));
			}


			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<Currency>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult<Currency>> UpdateCurrencyAsync (Currency currency, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context!.Currencies!.Update(currency);
			await context.SaveChangesAsync(cancellationToken);

			return TransactionResult<Currency>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("Error occured while updating currency with message: \n {message}", ex.InnerException?.Message ?? ex.Message);

			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
			{
				if (ex.InnerException!.Message.Contains("Name", StringComparison.InvariantCultureIgnoreCase))
					errors.Add(new(nameof(Currency.Name), UNIQUE_CURRENCY_NAME_ERROR));
				else if (ex.InnerException.Message.Contains("Code", StringComparison.InvariantCultureIgnoreCase))
					errors.Add(new(nameof(Currency.Code), UNIQUE_CURRENCY_CODE_ERROR));
			}

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<Currency>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult> DeleteCurrenciesAsync (params Currency[] currencies)
	{
		try
		{
			context!.Currencies!.RemoveRange(currencies);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			logger.LogError("Error occured while attempting to delete currency with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			IList<TransactionError> errors = [];
			if (IsForeignKeyViolation(ex))
				errors.Add(new("", currencies.Length > 1 ? "One or more currencies are currently in use." : "Currency is currently in use."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult.Failure([.. errors]);
		}
	}

	public async Task<Currency?> GetDefaultCurrencyAsync (CancellationToken cancellationToken = default)
		=> await context.Currencies!.Where(p => p.Default).FirstOrDefaultAsync(cancellationToken);




	/*****************************************************************
	 * BUSINESS SECTOR OPERATIONS
	 *****************************************************************/


	public async Task<IList<BusinessSector>> GetBusinessSectorsAsync (CancellationToken cancellationToken = default)
		=> await context!
			.BusinessSectors!
			.OrderBy(p => p.Name)
			.ToListAsync(cancellationToken);

	public async Task<BusinessSector?> FindBusinessSectorByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!
			.BusinessSectors!
			.FindAsync([id], cancellationToken);

	public async Task<TransactionResult<BusinessSector>> CreateAsync (BusinessSector sector, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.BusinessSectors!.AddAsync(sector, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<BusinessSector>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
				errors.Add(new(nameof(BusinessSector.Name), "Name is already taken."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<BusinessSector>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult<BusinessSector>> UpdateBusinessSectorAsync (BusinessSector sector, CancellationToken cancellationToken = default)
	{
		try
		{
			sector.RefreshConcurrencyToken();
			var result = context!.BusinessSectors!.Update(sector);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<BusinessSector>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
				errors.Add(new(nameof(BusinessSector.Name), "Name is already taken."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<BusinessSector>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult> DeleteBusinessSectorsAsync (params BusinessSector[] sectors)
	{
		try
		{
			context!.BusinessSectors!.RemoveRange(sectors);
			await context.SaveChangesAsync();

			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];

			if (DbExceptionUtils.IsForeignKeyViolation(ex))
				errors.Add(new(description: sectors.Length > 1 ? "One of more business sectors are currently in use." : "Business sector is currently in use."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult.Failure([.. errors]);
		}
	}


	/*****************************************************************
	 * DATE FORMATS OPERATIONS
	 *****************************************************************/

	private const string UNIQUE_FORMAT_ERROR = "Same date format already exists.";
	public async Task<TransactionResult<DateFormat>> CreateDateFormatAsync (DateFormat dateFormat, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.DateFormats!.AddAsync(dateFormat, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<DateFormat>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("Error occured while creating date fromat with message: \n {message}", ex.InnerException?.Message ?? ex.Message);

			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
				errors.Add(TransactionError.Create(nameof(DateFormat.Format), UNIQUE_FORMAT_ERROR));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<DateFormat>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult<DateFormat>> UpdateDateFormatAsync (DateFormat dateFormat, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context!.DateFormats!.Update(dateFormat);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<DateFormat>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("Error occured while updating datefromat with message: \n {message}", ex.InnerException?.Message ?? ex.Message);

			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
				errors.Add(TransactionError.Create(nameof(DateFormat.Format), UNIQUE_FORMAT_ERROR));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<DateFormat>.Failure([.. errors]);
		}
	}

	public async Task<DateFormat?> FindDateFormatByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.DateFormats!.FindAsync([id], cancellationToken);

	public async Task<TransactionResult> DeleteDateFormatsAsync (params DateFormat[] dateFormats)
	{
		try
		{
			context!.DateFormats!.RemoveRange(dateFormats);
			var result = await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			logger.LogError("Error occured while attempting to delete datefromat with message: \n {message}", ex.InnerException?.Message ?? ex.Message);
			IList<TransactionError> errors = [];

			if (IsForeignKeyViolation(ex))
				errors.Add(TransactionError.Create("", dateFormats.Length > 1 ? "One or more date formats are currently in use." : "The date format is curently in use."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult.Failure([.. errors]);
		}
	}

	public async Task<IList<DateFormat>> FindDateFormatsAsync (CancellationToken cancellationToken = default)
		=> await context!.DateFormats!.ToListAsync(cancellationToken);

	public async Task<DateFormat?> GetDefaultDateFormatAsync (CancellationToken cancellationToken = default)
		=> await context.DateFormats!.Where(p => p.Default).FirstOrDefaultAsync(cancellationToken);

	/*****************************************************************
	 * SHIPPING TERMS OPERATIONS
	 *****************************************************************/

	public async Task<IList<ShippingTerm>> GetShippingTermsAsync (CancellationToken cancellationToken = default)
		=> await context!.ShippingTerms!.ToListAsync(cancellationToken);

	public async Task<TransactionResult<ShippingTerm>> CreateShippingTermAsync (ShippingTerm term, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.ShippingTerms!.AddAsync(term, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<ShippingTerm>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (ex is DbUpdateException && ex.InnerException != null &&
				ex.InnerException.Message.Contains(DbUpdateErrors.UniqueIndexConstraint, StringComparison.InvariantCultureIgnoreCase))
				errors.Add(new(nameof(ShippingTerm.Name), "Shipping term already exists."));

			return TransactionResult<ShippingTerm>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult<ShippingTerm>> UpdateShippingTermAsync (ShippingTerm term, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context!.ShippingTerms!.Update(term);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<ShippingTerm>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (ex is DbUpdateException && ex.InnerException != null &&
				ex.InnerException.Message.Contains(DbUpdateErrors.UniqueIndexConstraint, StringComparison.InvariantCultureIgnoreCase))
				errors.Add(new(nameof(ShippingTerm.Name), "Shipping term already exists."));
			if (ex is DbUpdateConcurrencyException)
				errors.Add(new(description: "Something went wrong. Please try again."));

			return TransactionResult<ShippingTerm>.Failure([.. errors]);
		}
	}

	public async Task<ShippingTerm?> FindShippingTermByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.ShippingTerms!.FindAsync([id], cancellationToken);

	public async Task<TransactionResult> DeleteShippingTermsAsync (params ShippingTerm[] terms)
	{
		try
		{
			context!.ShippingTerms!.RemoveRange(terms);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (ex is DbUpdateException &&
				ex.InnerException != null &&
				ex.InnerException.Message.Contains(DbUpdateErrors.ForeignKeyConstaint, StringComparison.InvariantCultureIgnoreCase))
			{
				errors.Add(new(description: terms.Length > 1 ? "One of more shipping terms are currently in use." : "Shipping term is currently in use."));
			}

			return TransactionResult.Failure([.. errors]);
		}
	}


	/*****************************************************************
	 * SHIPPING METHOD OPERATIONS
	 *****************************************************************/
	public async Task<TransactionResult<ShippingMethod>> CreateShippingMethodAsync (ShippingMethod method, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.ShippingMethods!.AddAsync(method, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<ShippingMethod>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
				errors.Add(new(nameof(ShippingMethod.Name), "Name is already taken."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<ShippingMethod>.Failure();
		}
	}

	public async Task<TransactionResult<ShippingMethod>> UpdateShippingMethodAsync (ShippingMethod method, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context!.ShippingMethods!.Update(method);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<ShippingMethod>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (IsUniqueConstaint(ex))
				errors.Add(new(nameof(ShippingMethod.Name), "Shipping method already exists."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<ShippingMethod>.Failure();
		}
	}

	public async Task<ShippingMethod?> FindShippingMethodByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.ShippingMethods!.FindAsync([id], cancellationToken);

	public async Task<TransactionResult> DeleteShippingMethodsAsync (params ShippingMethod[] methods)
	{
		try
		{
			context!.ShippingMethods!.RemoveRange(methods);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];
			if (ex is DbUpdateException &&
				ex.InnerException != null &&
				ex.InnerException.Message.Contains(DbUpdateErrors.ForeignKeyConstaint, StringComparison.InvariantCultureIgnoreCase))
			{
				errors.Add(new(description: methods.Length > 1 ? "One of more shipping methods are currently in use." : "Shipping method is currently in use."));
			}

			return TransactionResult.Failure([.. errors]);
		}
	}

	public async Task<IList<ShippingMethod>> GetShippingMethodsAsync (CancellationToken cancellationToken = default)
		=> await context!.ShippingMethods!.ToListAsync(cancellationToken);

	/*****************************************************************
	 * PAYMENT METHOD OPERATIONS
	 *****************************************************************/
	public async Task<PaymentMethod?> FindPaymentMethodByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.PaymentMethods!.FindAsync([id], cancellationToken);

	public async Task<IList<PaymentMethod>> GetPaymentMethodsAsync (CancellationToken cancellationToken)
		=> await context!.PaymentMethods!.ToListAsync(cancellationToken);

	public async Task<TransactionResult<PaymentMethod>> CreatePaymentMethodAsync (PaymentMethod method, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.PaymentMethods!.AddAsync(method, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<PaymentMethod>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];

			if (DbExceptionUtils.IsUniqueKeyConstaint(ex))
				errors.Add(TransactionError.Create(nameof(PaymentMethod.Name), "Name is alreay in use."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<PaymentMethod>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult<PaymentMethod>> UpdatePaymentMethodAsync (PaymentMethod method, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context!.PaymentMethods!.Update(method);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<PaymentMethod>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];

			if (IsUniqueConstaint(ex))
				errors.Add(TransactionError.Create(nameof(PaymentMethod.Name), "Name is alreay in use."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<PaymentMethod>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult> DeletePaymentMethodsAsync (params PaymentMethod[] methods)
	{
		try
		{
			context!.PaymentMethods!.RemoveRange(methods);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];

			if (IsForeignKeyViolation(ex))
				errors.Add(TransactionError.Create("", methods.Length > 1 ? "Unable to delete methods that are currently in use." : "Method is currently in use."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult.Failure([.. errors]);
		}
	}

	/*****************************************************************
	 * PAYMENT TERM OPERATIONS
	 *****************************************************************/
	public async Task<TransactionResult<PaymentTerm>> CreatePaymentTermAsync (PaymentTerm term, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = await context!.PaymentTerms!.AddAsync(term, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<PaymentTerm>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];

			if (DbExceptionUtils.IsUniqueKeyConstaint(ex))
				errors.Add(new(nameof(PaymentTerm.Name), "Name is already taken."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<PaymentTerm>.Failure([.. errors]);
		}
	}

	public async Task<TransactionResult<PaymentTerm>> UpdatePaymentTermAsync (PaymentTerm term, CancellationToken cancellationToken = default)
	{
		try
		{
			var result = context!.PaymentTerms!.Update(term);
			await context.SaveChangesAsync(cancellationToken);
			return TransactionResult<PaymentTerm>.Success(result.Entity);
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];

			if (IsUniqueConstaint(ex))
				errors.Add(new(nameof(PaymentTerm.Name), "Payment term already exists."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult<PaymentTerm>.Failure([.. errors]);
		}
	}

	public async Task<PaymentTerm?> FindPaymentTermByIdAsync (int id, CancellationToken cancellationToken = default)
		=> await context!.PaymentTerms!.FindAsync([id], cancellationToken);

	public async Task<TransactionResult> DeletePaymentTermsAsync (params PaymentTerm[] terms)
	{
		try
		{
			context!.PaymentTerms!.RemoveRange(terms);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			IList<TransactionError> errors = [];

			if (IsForeignKeyViolation(ex))
				errors.Add(new("", terms.Length > 1 ? "One or more payment terms are currently in use." : "Payment term is currently in use."));

			if (errors.Any())
				errors.Add(GeneralError);

			return TransactionResult.Failure([.. errors]);
		}
	}

	public async Task<IList<PaymentTerm>> GetPaymentTermsAsync (CancellationToken cancellationToken = default)
		=> await context!.PaymentTerms!.ToListAsync(cancellationToken);

}
