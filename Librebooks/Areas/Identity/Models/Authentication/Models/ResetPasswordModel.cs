using FluentValidation;
using FluentValidation.Results;

namespace VectraBooks.Areas.Identity.Models.Authentication.Models
{
	public class ResetPasswordModel
	{
		public class Request
		{
			public string? Email { get; set; }
			public string? Password { get; set; }
			public string? Code { get; set; }
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

				RuleFor(p => p.Password)
					.NotEmpty().WithMessage("Password is required.");

				RuleFor(p => p.Code)
					.NotEmpty().WithMessage("Code is required.");
			}
		}
	}
}
