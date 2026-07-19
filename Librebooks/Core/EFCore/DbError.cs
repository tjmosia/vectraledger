using VectraBooks.Core.Operations;

namespace VectraBooks.Core.EFCore;

public class DbError
{
	public int ErrorNumber { get; set; }
	public TransactionError? Error { get; set; }
}
