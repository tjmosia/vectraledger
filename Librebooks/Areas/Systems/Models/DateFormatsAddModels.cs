using FluentValidation;
using FluentValidation.Results;

namespace VectraBooks.Areas.Systems.Models;

public class DateFormatsAddModels
{
	public class Request
	{
		public string? Format { get; set; }
	}

	public static ValidationResult Validate (Request model)
		=> new Validator().Validate(model);

	private class Validator : AbstractValidator<Request>
	{
		public Validator ()
		{
			RuleFor(p => p.Format)
				.Cascade(CascadeMode.Stop)
				.NotEmpty().WithMessage("Format is required.")
				.Must(p => p!.Length != 10).WithMessage("Invalid format.");
		}
	}
}

public class DateFormatsUpdateModel : DateFormatsAddModels
{

}