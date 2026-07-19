using VectraBooks.Core.EFCore;

using Microsoft.AspNetCore.Identity;

namespace VectraBooks.Core.Operations
{
	public class TransactionError : IdentityError
	{
		public TransactionError () { }

		public TransactionError (string message)
		{
			Code = "";
			Description = message;
		}

		public TransactionError (string code = "", string description = "")
		{
			Code = code;
			Description = description;
		}

		public static TransactionError FromIdentityError (IdentityError error)
			=> new(error.Code, error.Description);

		public static TransactionError FromDbError (DbError error)
			=> error.Error!;

		public static IEnumerable<TransactionError> FromIdentityErrors (params IdentityError[] errors)
		{
			foreach (var error in errors)
				yield return new TransactionError(error.Code, error.Description);
		}

		public static TransactionError FromIE (IdentityError error)
			=> FromIdentityError(error);

		public static IEnumerable<TransactionError> FromIEs (params IdentityError[] errors)
			=> FromIdentityErrors(errors);

		public static TransactionError Create (string code, string description = "")
			=> new(code, description);

		public static TransactionError Create (string description = "")
			=> new("", description);
	}
}
