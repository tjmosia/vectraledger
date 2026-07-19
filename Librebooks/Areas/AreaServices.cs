using VectraBooks.Areas.Accounting.Providers;
using VectraBooks.Areas.Companies.Services;
using VectraBooks.Areas.Inventory.Providers;
using VectraBooks.Areas.Systems.Providers;


//using VectraBooks.Areas.Accounting.Services;
//using VectraBooks.Areas.Customers.Services;
using VectraBooks.Core.EFCore;

namespace VectraBooks.Areas;

public static class AreaServices
{
	public static void ConfigureAll (IServiceCollection services)
	{
		services.AddSingleton<DbErrorDescriber>();
		services.AddScoped<ISystemsStore, SystemsStore>();
		services.AddScoped<ISystemsManager, SystemsManager>();
		services.AddScoped<ItemStore>();
		services.AddScoped<IItemManager, ItemManager>();

		services.AddScoped<ICompanyStore, CompanyStore>();
		services.AddScoped<ICompanyManager, CompanyManager>();

		services.AddScoped<IAccountsStore, AccountsStore>();
		services.AddScoped<IAccountingManager, AccountingManager>();
	}
}
