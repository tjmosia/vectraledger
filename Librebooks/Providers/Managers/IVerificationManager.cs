using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.GeneralSpace;

namespace VectraBooks.Providers.Managers
{
	public interface IVerificationManager
	{
		Task<(VerificationRequest? Request, string? Code)> AddAsync (VerificationRequest request);
		Task<TransactionResult<VerificationRequest>> VerifyAsync (string subject, string reason, string code);
	}
}
