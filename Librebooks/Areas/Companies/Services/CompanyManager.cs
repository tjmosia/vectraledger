using VectraBooks.Areas.Accounting.Providers;
using VectraBooks.Areas.Systems.Providers;
using VectraBooks.Core.Constants;
using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.DocumentSpace;
using VectraBooks.Models.Entity.IdentitySpace;
using VectraBooks.Models.Entity.InventorySpace;
using VectraBooks.Models.Entity.SupplierSpace;
using VectraBooks.Models.Entity.SystemSpace;
using VectraBooks.Providers.Stores;

namespace VectraBooks.Areas.Companies.Services;

public class CompanyManager (ICompanyStore store, SystemsStore systemsStore, DocumentSetupStore documentSetupStore, IAccountsStore? ledgerAccountStore) 
	: ICompanyManager
{
	private readonly ICompanyStore store = store;
	private readonly SystemsStore systemsStore = systemsStore;
	private readonly DocumentSetupStore documentSetupStore = documentSetupStore;
	private readonly IAccountsStore? ledgerAccountStore = ledgerAccountStore;

	public async Task<TransactionResult<Company>> CreateAsync (Company company, User user, Country country, Currency currency)
	{
		company.CustomerSetup = new CustomerSetup
		{
			Prefix = "CUS",
			NextNumber = 1
		};

		company.SupplierSetup = new SupplierSetup
		{
			Prefix = "SUP",
			NextNumber = 1,
		};

		company.ItemSetup = new ItemSetup
		{
			Prefix = "ITM",
			NextNumber = 1
		};

		company.RegionalSetup = new CompanyRegionalSetup
		{
			DateFormat = await systemsStore.GetDefaultDateFormatAsync(),
			Currency = currency,
			Country = country,
			DecimalMark = ".",
			ThousandsSeperator = " ",
			RoundToNearest = 2
		};

		company.DocumentSetups = [..(await documentSetupStore.FindAllAsync()).Select( p=> new DocumentSetup
		{
			FooterMessage = p.FooterMessage,
			NoteMessage = p.NoteMessage,
			Prefix = p.Prefix,
			NextNumber = p.NextNumber,
			Suffix = p.Suffix,
			System = false,
			TypeId = p.TypeId,
			PrintTemplateId = p.PrintTemplateId,
			Title = p.Title
		})];

		company.Users = [new CompanyUser{
			UserId = user.Id
		}];

		company.Taxes = [..(await systemsStore.GetTaxesAsync()).Select(p => new CompanyTax
		{
			Tax = p,
			Default = company.VATNumber == null && p.Type!.Equals(TaxCodeTypes.ZeroVAT) || p.Type!.Equals(TaxCodeTypes.StandardVAT)
		})];

		company.ChartOfAccounts = [.. (await ledgerAccountStore!.FindBySysAsync()).Select(p=> new CompanyLedgerAccount {
			Account = p,
			Balance = 0
		})];

		return await store.CreateAsync(company);
	}
}
