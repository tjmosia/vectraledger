using VectraBooks.Data;

namespace VectraBooks.Areas.Customers.Providers
{
	public class SalesManager (AppDbContext context, ISalesStore store) : ISalesManager
	{
		private readonly AppDbContext context = context;
		private readonly ISalesStore store = store;


	}
}
