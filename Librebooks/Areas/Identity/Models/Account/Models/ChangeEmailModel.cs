using FluentValidation;
using FluentValidation.Results;

namespace VectraBooks.Areas.Identity.Models.Account.Models
{
	public class ChangeEmailModel
	{
		public class Request
		{
			public string? Email { get; set; }
		}

		public static ValidationResult Validate (Request request)
			=> new Validator().Validate(request);

		private class Validator : AbstractValidator<Request>
		{
			public Validator ()
			{
				RuleFor(p => p.Email)
					.Cascade(CascadeMode.Stop)
					.NotEmpty().WithMessage("Email is required.")
					.EmailAddress().WithMessage("Please check your email.");
			}
		}
	}
}
