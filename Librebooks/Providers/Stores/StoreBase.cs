using VectraBooks.Core.Constants;
using VectraBooks.Core.Operations;
using VectraBooks.Data;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Providers.Stores;

public abstract class StoreBase (AppDbContext context)
{
	protected readonly AppDbContext context = context;

	private readonly TransactionError ConcurrencyError = new(description: "Something went wrong. Please try agian.");
	public static bool IsForeignKeyViolation (Exception ex)
		=> ex is DbUpdateException &&
			ex.InnerException != null &&
			ex.InnerException.Message.Contains(DbUpdateErrors.ForeignKeyConstaint, StringComparison.InvariantCultureIgnoreCase);

	public static bool IsUniqueKeyConstaint (Exception ex)
		=> ex is DbUpdateException && ex.InnerException != null &&
			ex.InnerException.Message.Contains(DbUpdateErrors.UniqueIndexConstraint, StringComparison.InvariantCultureIgnoreCase);
}
