namespace VectraBooks.Core.Identity
{
	public class AppClaimTypes
	{
		public static class Roles
		{
			public const string Master = "app/claims/roles/master";
			public const string Admin = "app/claims/roles/admin";
			public const string User = "app/claims/roles/user";
			public const string Accountant = "app/claims/roles/accountant";
			public const string CompanyUser = "app/claims/roles/company_user";
		}

		public static class Modules
		{
			public const string Accounting = "app/claims/modules/accounting";
			public const string Inventory = "app/claims/modules/inventory";
			public const string Selling = "app/claims/modules/selling";
			public const string Purchasing = "app/claims/modules/purchasing";
		}
	}
}
