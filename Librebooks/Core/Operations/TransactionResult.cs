
namespace VectraBooks.Core.Operations
{
	/// <summary>
	/// Returns a summary result of a transaction associated with <typeparamref name="TModel"/>.
	/// </summary>
	/// <typeparam name="TModel">Is the class type of the model instance associated with he transaction.</typeparam>
	public class TransactionResult<TModel>
		where TModel : class
	{
		public readonly bool Succeeded;
		public readonly TModel? Model;
		public readonly TransactionError[] Errors;

		private TransactionResult (bool succeeded, TransactionError[] errors, TModel? model)
		{
			Succeeded = succeeded;
			Model = model;
			Errors = errors;
		}

		public static TransactionResult<TModel> Success (TModel? Model = null)
			=> new(true, [], Model);

		public static TransactionResult<TModel> Failure (params TransactionError[] errors)
			=> new(false, errors, null);

		public static TransactionResult<TModel> Failure (Func<TransactionError> ErrorsExpression)
			=> new(false, [ErrorsExpression()], null);

		internal void Deconstruct (out object Succeeded, out TModel? Model, out TransactionError[] Errors)
		{
			Succeeded = this.Succeeded;
			Model = this.Model;
			Errors = this.Errors;
		}
	}

	/// <summary>
	/// Returns a summary result of a transaction.
	/// </summary>
	public class TransactionResult
	{
		public readonly bool Succeeded;
		public readonly TransactionError[] Errors;
		public static readonly TransactionResult Success = new(true, []);

		private TransactionResult (bool succeeded, TransactionError[] errors)
		{
			Succeeded = succeeded;
			Errors = errors;
		}

		public static TransactionResult Failure (params TransactionError[] errors)
			=> new(false, errors);

		public static TransactionResult Failure (Func<TransactionError> ErrorsExpression)
			=> new(false, [ErrorsExpression()]);
	}
}
