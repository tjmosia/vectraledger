using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.GeneralSpace;

namespace VectraBooks.Providers.Managers
{
	public class VerificationManager (VerificationStore store, ILogger<VerificationManager> logger) : IVerificationManager
	{
		private readonly VerificationStore store = store;
		private readonly ILogger<VerificationManager> logger = logger;

		/**********************************************************************************************************************
		 *	AddAsync
		 *********************************************************************************************************************/
		public async Task<(VerificationRequest? Request, string? Code)> AddAsync (VerificationRequest request)
		{
			request.Email = NormalizeEmail(request.Email!);
			request.Reason = NormalizeReason(request.Reason!);

			var _request = await store.FindAsync(request.Email!, request.Reason!);

			if (_request != null)
				await store.DeleteAsync(_request);

			var code = GenerateRandomCode();

			request.HashString = GenerateHashString(request.Email, request.Reason, code);
			var newRequest = await store.CreateAsync(request);

			return (newRequest, code);
		}

		/**********************************************************************************************************************
		 *	VerifyAsync
		 *********************************************************************************************************************/
		public async Task<TransactionResult<VerificationRequest>> VerifyAsync (string email, string reason, string code)
		{
			var request = await store.FindAsync(email, reason);

			if (request != null)
			{
				var confirmed = BCrypt.Net.BCrypt.Verify(string.Concat(NormalizeEmail(email), NormalizeEmail(reason), code), request.HashString);
				logger.LogInformation("Verification Result returned **********************************************************************");

				if (!confirmed)
				{
					if (request.Attempts != request.MaxAttemptsAllowed)
					{
						request.Attempts += 1;
						request.RefreshConcurrencyToken();
						await store.UpdateAsync(request);
						return TransactionResult<VerificationRequest>.Failure(TransactionError.Create("Code", "Code is invalid."));
					}
					await store.DeleteAsync(request);
				}
				else
				{
					return TransactionResult<VerificationRequest>.Success(request);
				}
			}

			return TransactionResult<VerificationRequest>.Failure(TransactionError.Create("", "Request a new verification code."));
		}

		private static string GenerateHashString (string email, string reason, string code)
			=> BCrypt.Net.BCrypt.HashPassword(string.Concat(NormalizeEmail(email), NormalizeEmail(reason), code));

		private static string GenerateRandomCode ()
			=> new Random().Next(100000, 999999).ToString("000000");

		private static string NormalizeEmail (string email)
		{
			return email.ToLower();
		}

		private static string NormalizeReason (string reason)
		{
			return reason.ToUpper();
		}
	}
}
