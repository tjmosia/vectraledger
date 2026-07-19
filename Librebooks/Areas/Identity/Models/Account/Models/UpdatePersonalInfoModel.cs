using FluentValidation;
using FluentValidation.Results;

namespace VectraBooks.Areas.Identity.Models.Account.Models;

public class UpdatePersonalInfoModel
{
	public class Request
	{
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Gender { get; set; }
	}

	public static ValidationResult Validate (Request request)
		=> new Validator().Validate(request);

	private class Validator : AbstractValidator<Request>
	{
		public Validator ()
		{
			RuleFor(p => p.Name)
				.NotEmpty().WithMessage("First name is required.");

			RuleFor(p => p.Surname)
				.NotEmpty().WithMessage("Last name is required.");

			RuleFor(p => p.Gender)
				.NotEmpty().WithMessage("Gender is required.");
		}
	}
}