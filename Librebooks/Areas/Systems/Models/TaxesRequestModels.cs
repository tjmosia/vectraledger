using FluentValidation;
using FluentValidation.Results;
using static VectraBooks.Core.Constants.ApplicationTypes;

namespace VectraBooks.Areas.Systems.Models;

public class TaxAddModel
{
	public class Request
	{
		public string? Name { get; set; }
		public decimal Rate { get; set; }
		public bool System { get; set; } = false;
		public string? Type { get; set; }

	}

	public static ValidationResult Validate (Request request)
		=> new Validator().Validate(request);

	private class Validator : AbstractValidator<Request>
	{
		public Validator ()
		{
			RuleFor(p => p.Name)
				.NotNull().WithMessage("Tax name is required.");

			RuleFor(p => p.Rate)
				.Cascade(CascadeMode.Stop)
				.Must(p => p <= 100).WithMessage("Tax rate cannot not be more than 100%.")
				.Must(p => p >= 0).WithMessage("Tax rate cannot be less than 0.");

			RuleFor(p => p.Type)
				.Cascade(CascadeMode.Stop)
				.NotNull().WithMessage("Tax type is required.")
				.Must(p => p == TaxTypes.ValueAdded || p == TaxTypes.Income).WithMessage("Invalid tax type.");
		}
	}
}

public class TaxUpdateModel : TaxAddModel
{

}
