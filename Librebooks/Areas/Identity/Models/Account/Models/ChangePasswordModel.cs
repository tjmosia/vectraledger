using FluentValidation;
using FluentValidation.Results;

namespace VectraBooks.Areas.Identity.Models.Account.Models
{
	public class ChangePasswordModel
	{
		public class Request
		{
			public string? OldPassword { get; set; }
			public string? Password { get; set; }
		}

		public static ValidationResult Validate (Request request)
			=> new Validator().Validate(request);

		private class Validator : AbstractValidator<Request>
		{
			public Validator ()
			{
				RuleFor(p => p.OldPassword)
					.NotEmpty().WithMessage("Current password is required.");

				RuleFor(p => p.Password)
					.NotEmpty().WithMessage("New password is required.");
			}
		}
	}
}
