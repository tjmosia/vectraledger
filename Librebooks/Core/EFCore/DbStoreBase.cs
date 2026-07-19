using VectraBooks.Core.Constants;
using VectraBooks.Core.Operations;
using VectraBooks.Data;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Core.EFCore
{
	public abstract class DbStoreBase
		(AppDbContext context)
	{
		protected readonly AppDbContext context = context;

		protected readonly TransactionError GeneralError =
			new("Something went wrong. Please try agian.");

		protected static bool IsForeignKeyViolation (Exception ex)
			=> ex is DbUpdateException &&
				ex.InnerException != null &&
				ex.InnerException.Message.Contains(DbUpdateErrors.ForeignKeyConstaint, StringComparison.InvariantCultureIgnoreCase);

		protected static bool IsUniqueConstaint (Exception ex)
			=> ex is DbUpdateException && ex.InnerException != null &&
				ex.InnerException.Message.Contains(DbUpdateErrors.UniqueIndexConstraint, StringComparison.InvariantCultureIgnoreCase);

		protected static string GenerateRowVersion ()
			=> Guid.NewGuid().ToString("N").ToUpper();
    }
}