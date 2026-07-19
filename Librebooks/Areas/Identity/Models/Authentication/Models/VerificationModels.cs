using FluentValidation;
using FluentValidation.Results;

namespace VectraBooks.Areas.Identity.Models.Authentication.Models;

public class VerificationModels
{
	public class Create
	{
		public class Request
		{
			public string? Email { get; set; }
			public string? Reason { get; set; }
		}

		public static ValidationResult Validate (Request request)
			=> new Validator().Validate(request);

		internal class Validator : AbstractValidator<Request>
		{
			public Validator ()
			{
				RuleFor(p => p.Email)
					.NotEmpty().WithMessage("Email is required")
					.EmailAddress().WithMessage("Please check your email.");

				RuleFor(p => p.Reason)
					.NotEmpty().WithMessage("Reason is required");
			}
		}
	}
	public class Verify
	{
		public class Request : Create.Request
		{
			public string? Code { get; set; }
		}

		public static ValidationResult Validate (Request request)
			=> new Validator().Validate(request);

		public class Validator : AbstractValidator<Request>
		{
			public Validator ()
			{
				Include(new Create.Validator());

				RuleFor(p => p.Code)
					.Cascade(CascadeMode.Stop)
					.NotEmpty().WithMessage("Code is required")
					.Length(6).WithMessage("Code must be 6 characters long");
			}
		}
	}
}
