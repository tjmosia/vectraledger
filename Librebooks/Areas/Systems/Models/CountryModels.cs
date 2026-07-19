using FluentValidation;
using FluentValidation.Results;
using VectraBooks.Models.Entity.SystemSpace;

namespace VectraBooks.Areas.Systems.Models;

public class CountryModel
{
	public class Create
	{
		public class Request
		{
			public string? Code { get; set; }
			public string? Name { get; set; }
			public string? DialingCode { get; set; }

			public Country MapToCountry (Country country)
			{
				country.Code = Code;
				country.Name = Name;
				country.DialingCode = DialingCode;
				return country;
			}
		}

		public static ValidationResult Validate (Request request)
			=> new Validator().Validate(request);

		protected internal class Validator : AbstractValidator<Request>
		{
			public Validator ()
			{
				RuleFor(x => x.Code)
					.Cascade(CascadeMode.Stop)
					.NotEmpty().WithMessage("Country code is required.")
					.Length(3).WithMessage("Code must be 3 characters long.");

				RuleFor(x => x.Name)
					.NotEmpty().WithMessage("Country name is required.");
			}
		}
	}

	public class Update
	{
		public class Request : Create.Request
		{
		}

		public static ValidationResult Validate (Request request)
			=> new Validator().Validate(request);

		protected internal class Validator : AbstractValidator<Request>
		{
			public Validator ()
			{
				Include(new Create.Validator());
			}
		}

	}
}
