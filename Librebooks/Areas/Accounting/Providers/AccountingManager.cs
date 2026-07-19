using VectraBooks.Data;
using VectraBooks.Providers.Stores;

namespace VectraBooks.Areas.Accounting.Providers;

public class AccountingManager (AppDbContext context) : StoreBase(context), IAccountingManager
{


}
