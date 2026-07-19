using VectraBooks.Core.Operations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Core.EFCore;

public class AppErrorDescriber
{
	public static TransactionError GetErrorFromDbException (Exception ex, string location, ILogger? logger)
	{
		logger?.LogError("Exception Occured at {location}: \n {message}", location, ex.Message);

		var code = "";

		if (ex is DbUpdateConcurrencyException)
			code = nameof(AppErrorDescriber.Concurrency);
		if (ex is DbUpdateException && ex.InnerException != null && ex.InnerException is SqlException sqlEx)
		{
			var message = sqlEx.Message.ToUpper();
			if (message.Contains("UNIQUE_INDEX"))
				code = nameof(AppErrorDescriber.UniqueKeyOrIndexConstraint);
			else if (message.Contains("FOREIGN_KEY"))
				code = nameof(AppErrorDescriber.ForeignKeyConstraint);
		}

		return new TransactionError(code, "");
	}

	public static TransactionError Unknown (string message = "")
		=> new(nameof(Unknown), message);

	public static TransactionError DuplicateKey (string errorMessage = "")
		=> new(nameof(DuplicateKey), errorMessage);

	public static TransactionError UniqueKeyOrIndexConstraint (string errorMessage = "")
		=> new(nameof(UniqueKeyOrIndexConstraint), errorMessage);

	public static TransactionError ForeignKeyConstraint (string errorMessage = "")
		=> new(nameof(ForeignKeyConstraint), errorMessage);

	public static TransactionError Concurrency (string errorMessage = "")
		=> new(nameof(Concurrency), errorMessage);
}
