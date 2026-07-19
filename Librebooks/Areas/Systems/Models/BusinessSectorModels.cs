using FluentValidation;
using FluentValidation.Results;
using VectraBooks.Models.Entity.SystemSpace;

namespace VectraBooks.Areas.Systems.Models;

public class BusinessSectorRequestModels
{
	public class Create
	{
		public class Request
		{
			public string? Name { get; set; }

			public void MapToBusinessSector (BusinessSector sector)
			{
				sector.Name = Name;
			}
		}

		public static ValidationResult Validate (Request request)
			=> new Validator().Validate(request);

		protected internal class Validator : AbstractValidator<Request>
		{
			public Validator ()
			{
				RuleFor(x => x.Name)
					.NotEmpty().WithMessage("Name is required.");
			}
		}
	}
}
