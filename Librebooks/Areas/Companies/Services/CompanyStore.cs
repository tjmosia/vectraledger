using Librebooks.Areas.Systems.Providers;
using Librebooks.Core.EFCore;
using Librebooks.Core.Operations;
using Librebooks.Data;
using Librebooks.Models.Entity.BankingSpace;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;
using Librebooks.Models.Entity.DocumentSpace;
using Librebooks.Models.Entity.GeneralSpace;
using Librebooks.Models.Entity.IdentitySpace;
using Librebooks.Models.Entity.InventorySpace;
using Librebooks.Models.Entity.SupplierSpace;
using Librebooks.Models.Entity.SystemSpace;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Librebooks.Areas.Companies.Services;

public partial class CompanyStore(AppDbContext context, ILogger<CompanyStore> logger)
    : DbStoreBase(context), ICompanyStore
{
    private readonly ILogger<CompanyStore> logger = logger;

    public async Task<Company?> FindByIdAsync(int companyId, CancellationToken cancellationToken = default)
        => await context!.Companies!
            .Where(p => p.Id == companyId)
            .Include(p => p.Logo)
                .ThenInclude(p => p!.Image)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Company?> FindByIdAsync(int companyId, int userId, CancellationToken cancellationToken = default)
    {
        return await context.CompanyUsers!.Where(p => p.CompanyId == companyId && p.UserId == userId)
            .Include(p => p.Company)
            .ThenInclude(p => p!.Logo)
                .ThenInclude(p => p!.Image)
            .Select(p => p.Company)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<Company?>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await context.CompanyUsers!.Where(p => p.UserId == userId)
            .Include(p => p.Company)
            .ThenInclude(p => p!.Logo)
                .ThenInclude(p => p!.Image)
            .Select(p => p.Company)
            .ToListAsync(cancellationToken);
    }

    public async Task<CompanyRegionalSetup?> GetRegionalSettingsAsync(Company company, CancellationToken cancellationToken = default)
        => await context!.CompanyRegionalSettings!
            .FindAsync([company.Id], cancellationToken);

    public async Task<CompanyImage?> GetLogoAsync(Company company, CancellationToken cancellationToken = default)
        => await context!.CompanyLogos!
            .Where(p => p.CompanyId == company.Id)
            .Include(p => p.Image)
            .Select(p => p.Image)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IList<Tax>> GetTaxesAsync(Company company, CancellationToken cancellationToken = default)
        => await context!.CompanyTaxes!
            .Where(p => p.CompanyId == company.Id)
            .Include(p => p.Tax)
            .Select(p => p.Tax!)
            .ToListAsync(cancellationToken);

    public async Task<Tax?> FindTaxByIdAsync(Company company, int taxTypeId, CancellationToken cancellationToken = default)
        => await context!.CompanyTaxes!
            .Where(p => p.CompanyId == company.Id && p.TaxId == taxTypeId)
            .Include(p => p.Tax)
            .Select(p => p.Tax)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Tax?> GetDefaultTaxAsync(Company company, CancellationToken cancellationToken = default)
        => await context!.CompanyTaxes!
            .Where(p => p.CompanyId == company.Id && p.Default)
            .Include(p => p.Tax)
            .Select(p => p.Tax)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<CompanyMailSetup?> GetMailSettingsAsync(Company company, CancellationToken cancellationToken = default)
        => await context!.CompanyMailSettings!
            .FindAsync([company.Id], cancellationToken);

    public async Task<BankAccount?> GetDefaultBankAccountAsync(Company company, CancellationToken cancellationToken = default)
     => await context!.CompanyDefaultBankAccounts!
            .Where(p => p.CompanyId == company.Id)
            .Include(p => p.BankAccount)
            .Select(p => p.BankAccount)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<BankAccount?> FindBankAccountByIdAsync(Company company, int bankAccountId, CancellationToken cancellationToken = default)
        => await context!.BankAccounts!
            .Where(p => p.CompanyId == company.Id && bankAccountId == p.Id)
            .FirstOrDefaultAsync(cancellationToken);
    public async Task<IList<BankAccount>> GetBankAccountsAsync(Company company, CancellationToken cancellationToken = default)
        => await context!.BankAccounts!
            .Where(p => p.CompanyId == company.Id)
            .ToListAsync(cancellationToken);

    public async Task<Contact?> FindSalesPersonByIdAsync(Company company, int salesPersonId, CancellationToken cancellationToken = default)
        => await context!.SalesPeople!
            .Where(p => p.CompanyId == company.Id && p.ContactId == salesPersonId)
            .Include(p => p.Contact)
            .Select(p => p.Contact)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Contact?> FindSalesPersonByUserIdAsync(Company company, int userId, CancellationToken cancellationToken = default)
        => await context!.CompanyUsers!
            .Where(p => p.CompanyId == company.Id && p.UserId == userId)
            .Include(p => p.SalesPerson)
                .ThenInclude(p=>p.Contact)
            .Select(p => p.SalesPerson!.Contact)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IList<User>> GetUsersAsync(Company company, CancellationToken cancellationToken = default)
        => await context!.CompanyUsers!
            .Where(p => p.CompanyId == company.Id)
            .Include(p => p.User)
            .Select(p => p.User!)
            .ToListAsync(cancellationToken);

    /************************************************************************
	 * CREATE OPERATIONS
	 ************************************************************************/

    public async Task<TransactionResult<Company>> CreateAsync(Company company)
    {
        try
        {
            var result = await context!.AddAsync(company);
            context.SaveChanges();

            return TransactionResult<Company>.Success(result.Entity);
        }
        catch (Exception)
        {
            return TransactionResult<Company>.Failure(() =>
            {
                return GeneralError;
            });
        }
    }

    public async Task<TransactionResult<CompanyTax>> AddTaxAsync(Company company, Tax tax)
    {
        try
        {
            var result = await context.CompanyTaxes!.AddAsync(new CompanyTax(company.Id, tax.Id));
            await context.SaveChangesAsync();

            return TransactionResult<CompanyTax>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<CompanyTax>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(AddTaxAsync), logger));
        }
    }

    public async Task<TransactionResult<Contact>> AddSalesPersonAsync(Company company, Contact contact)
    {
        try
        {
            var result = await context.Contacts!.AddAsync(contact);

            await context.SalesPeople!.AddAsync(new CompanySalesRep
            {
                CompanyId = company.Id,
                ContactId = contact.Id,
            });

            await context.SaveChangesAsync();

            return TransactionResult<Contact>.Success(result.Entity);
        }
        catch (Exception ex)
        {

            return TransactionResult<Contact>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(AddSalesPersonAsync), logger));
        }
    }

    public async Task<TransactionResult<BankAccount>> AddBankAccountAsync(Company company, BankAccount bankAccount)
    {
        try
        {
            bankAccount.CompanyId = company.Id;
            var result = await context.BankAccounts!.AddAsync(bankAccount);
            await context.SaveChangesAsync();
            return TransactionResult<BankAccount>.Success(result.Entity);
        }
        catch (Exception ex)
        {

            return TransactionResult<BankAccount>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(AddBankAccountAsync), logger));
        }
    }

    public async Task<TransactionResult<BankAccount>> AddDefaultBankAccountAsync(Company company, BankAccount bankAccount)
    {
        try
        {


            var result = await context.CompanyDefaultBankAccounts!
                .AddAsync(new CompanyBankAccount(company.Id, bankAccount.Id));
            await context.SaveChangesAsync();
            return TransactionResult<BankAccount>.Success(bankAccount);
        }
        catch (Exception ex)
        {

            return TransactionResult<BankAccount>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(AddDefaultBankAccountAsync), logger));
        }
    }

    public async Task<TransactionResult<CompanyImage>> AddLogoAsync(Company company, CompanyImage image)
    {
        try
        {
            var result = await context.CompanyImages!.AddAsync(image);
            await context.SaveChangesAsync();
            await UpdateLogoAsync(company, result.Entity);
            return TransactionResult<CompanyImage>.Success(result.Entity);
        }
        catch (Exception ex)
        {

            return TransactionResult<CompanyImage>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(AddLogoAsync), logger));
        }
    }

    public async Task<TransactionResult<CompanyMailSetup>> AddMailSettingsAsync(Company company, CompanyMailSetup companyMailSetup)
    {
        try
        {
            companyMailSetup.CompanyId = company.Id;
            var add = await context.CompanyMailSettings!.AddAsync(companyMailSetup);
            await context.SaveChangesAsync();
            return TransactionResult<CompanyMailSetup>.Success(add.Entity);
        }
        catch (Exception)
        {
            return TransactionResult<CompanyMailSetup>.Failure();
        }
    }

    /************************************************************************
	 * UPDATE OPERATIONS
	 ************************************************************************/

    public async Task<TransactionResult<Company>> UpdateAsync(Company company)
    {
        company.RefreshConcurrencyToken();

        try
        {
            var result = context.Companies!.Update(company);
            await context.SaveChangesAsync();
            return TransactionResult<Company>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<Company>.Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateAsync), logger));
        }
    }

    public async Task<TransactionResult<CompanyRegionalSetup>> UpdateRegionalSettingsAsync(CompanyRegionalSetup regionalSettings)
    {
        try
        {
            var result = context!.CompanyRegionalSettings!.Update(regionalSettings);
            await context.SaveChangesAsync();
            return TransactionResult<CompanyRegionalSetup>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<CompanyRegionalSetup>.Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateRegionalSettingsAsync), logger));
        }
    }

    public async Task<TransactionResult<CompanyBankAccount>> UpdateDefaultTaxAsync(CompanyTax defaultTax)
    {
        try
        {
            defaultTax.Default = true;
            context!.CompanyTaxes!.Update(defaultTax);
            await context!.SaveChangesAsync();
            return TransactionResult<CompanyBankAccount>.Success();
        }
        catch (Exception ex)
        {
            return TransactionResult<CompanyBankAccount>.Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateDefaultTaxAsync), logger));
        }
    }

    public async Task<TransactionResult<CompanyTax>> UpdateTaxAsync(CompanyTax tax)
    {
        try
        {
            var result = context!.CompanyTaxes!.Update(tax);
            await context.SaveChangesAsync();
            return TransactionResult<CompanyTax>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<CompanyTax>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateTaxAsync), logger));
        }
    }

    public async Task<TransactionResult<CompanyImage>> UpdateLogoAsync(Company company, CompanyImage companyImage)
    {
        try
        {
            var image = await context!.CompanyImages!.AddAsync(companyImage);
            var companyLogo = await context.CompanyLogos!.FindAsync(company.Id!);

            if (companyLogo == null)
            {
                await context!.CompanyLogos!.AddAsync(new CompanyLogo(company.Id!, image.Entity.Id));
            }
            else
            {
                companyLogo.ImageId = image.Entity.CompanyId;
                context!.CompanyLogos!.Update(companyLogo);
            }

            await context!.SaveChangesAsync();
            return TransactionResult<CompanyImage>.Success(image.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<CompanyImage>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateLogoAsync), logger));
        }
    }

    public async Task<TransactionResult> UpdateMailSettingsAsync(CompanyMailSetup companyMailSettings)
    {
        try
        {
            context!.CompanyMailSettings!.Update(companyMailSettings);
            await context!.SaveChangesAsync();
            return TransactionResult.Success;
        }
        catch (Exception ex)
        {
            return TransactionResult
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateMailSettingsAsync), logger));
        }
    }

    public async Task<TransactionResult<SupplierSetup>> UpdateSupplierSetupAsync(SupplierSetup supplierSetup)
    {
        try
        {
            var result = context.SupplierSetups!.Update(supplierSetup);
            await context.SaveChangesAsync();
            return TransactionResult<SupplierSetup>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<SupplierSetup>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateSupplierSetupAsync), logger));
        }
    }

    public async Task<TransactionResult<CustomerSetup>> UpdateCustomerSetupAsync(CustomerSetup customerSetup)
    {
        try
        {
            var result = context.CustomerSetups!.Update(customerSetup);
            await context.SaveChangesAsync();
            return TransactionResult<CustomerSetup>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<CustomerSetup>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateCustomerSetupAsync), logger));
        }
    }

    public async Task<TransactionResult<ItemSetup>> UpdateItemSetupAsync(ItemSetup itemSetup)
    {
        try
        {
            var result = context.ItemSetups!.Update(itemSetup);
            await context.SaveChangesAsync();
            return TransactionResult<ItemSetup>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<ItemSetup>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateItemSetupAsync), logger));
        }
    }

    public async Task<TransactionResult<DocumentSetup>> UpdateDocumentSetupAsync(DocumentSetup documentSetup)
    {
        try
        {
            var result = context.DocumentSetups!.Update(documentSetup);
            await context.SaveChangesAsync();
            return TransactionResult<DocumentSetup>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<DocumentSetup>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateDocumentSetupAsync), logger));
        }
    }

    public async Task<TransactionResult<BankAccount>> UpdateBankAccountAsync(BankAccount bankAccount)
    {
        try
        {
            var result = context.BankAccounts!.Update(bankAccount);
            await context.SaveChangesAsync();
            return TransactionResult<BankAccount>.Success(result.Entity);
        }
        catch (Exception ex)
        {
            return TransactionResult<BankAccount>
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(UpdateBankAccountAsync), logger));
        }
    }

    /************************************************************************
	 * DELETE OPERATIONS
	 ************************************************************************/

    public async Task<TransactionResult> DeleteSalesPersonAsync(CompanySalesRep salesPerson)
    {
        try
        {
            context.SalesPeople!.Remove(salesPerson);
            context.Contacts!.Remove(salesPerson.Contact!);
            await context!.SaveChangesAsync();

            return TransactionResult.Success;
        }
        catch (Exception ex)
        {
            return TransactionResult
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(DeleteSalesPersonAsync), logger));
        }
    }

    public async Task<TransactionResult> DeleteAsync(Company company)
    {

        try
        {
            context.Companies!.Remove(company);
            await context.SaveChangesAsync();
            return TransactionResult.Success;
        }
        catch (Exception ex)
        {
            return TransactionResult
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(DeleteAsync), logger));
        }
    }

    public async Task<TransactionResult> DeleteTaxAsync(CompanyTax companyTaxType)
    {
        try
        {
            context.CompanyTaxes!.Remove(companyTaxType);
            await context.SaveChangesAsync();

            return TransactionResult.Success;
        }
        catch (Exception ex)
        {
            return TransactionResult
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(DeleteTaxAsync), logger));
        }
    }

    public async Task<TransactionResult> DeleteBankAccountAsync(BankAccount bankAccount)
    {
        try
        {
            context.BankAccounts!.Remove(bankAccount);
            await context.SaveChangesAsync();
            return TransactionResult.Success;
        }
        catch (Exception ex)
        {
            return TransactionResult
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(DeleteBankAccountAsync), logger));
        }
    }

    public async Task<TransactionResult> DeleteLogoAsync(CompanyLogo companyLogo)
    {
        try
        {
            context.CompanyLogos!.Remove(companyLogo);
            await context.SaveChangesAsync();
            return TransactionResult.Success;
        }
        catch (Exception ex)
        {
            return TransactionResult
                .Failure(AppErrorDescriber.GetErrorFromDbException(ex, nameof(DeleteLogoAsync), logger));
        }
    }
}
