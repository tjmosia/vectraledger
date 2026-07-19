using VectraBooks.Core.EFCore;
using VectraBooks.Data;

namespace VectraBooks.Areas.Customers.Providers
{
    public class SalesStore (AppDbContext context): DbStoreBase(context), ISalesStore
    {

    }
}
