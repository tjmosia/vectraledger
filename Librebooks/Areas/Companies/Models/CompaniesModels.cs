using FluentValidation;
using FluentValidation.Results;

namespace VectraBooks.Areas.Companies.Models;

public class CompaniesModels
{
	public class Request
	{
		public virtual string? LegalName { get; set; }
		public virtual string? TradingName { get; set; }
		public virtual string? RegNumber { get; set; }
		public virtual string? VATNumber { get; set; }
		public virtual bool VATRegistered { get; set; }
		public virtual string? PhysicalAddress { get; set; }
		public virtual string? PostalAddress { get; set; }
		public virtual string? TelephoneNumber { get; set; }
		public virtual string? Email { get; set; }
		public virtual string? FaxNumber { get; set; }
		public virtual int BusinessSectorId { get; set; }
		public virtual string? Logo { get; set; }
		public virtual int CurrencyId { get; set; }
		public virtual int CountryId { get; set; }
		public virtual int DateFormatId { get; set; }
	}

	public static ValidationResult Validate (Request request)
		=> new Validator().Validate(request);

	private class Validator : AbstractValidator<Request>
	{
		public Validator ()
		{
			RuleFor(x => x.LegalName)
				.NotEmpty()
				.WithMessage("Registered name is required");

			RuleFor(x => x.TradingName)
				.NotEmpty()
				.WithMessage("Trading name is required");

			RuleFor(x => x.PhysicalAddress)
				.NotEmpty()
				.WithMessage("Physicall address is required");

			RuleFor(x => x.PostalAddress)
				.NotEmpty()
				.WithMessage("Postal address is required");

			RuleFor(x => x.TelephoneNumber)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("Telephone number is required")
				.Must(Telephone => BeAValidPhoneNumber(Telephone))
				.WithMessage("Invalid telephone number format.");

			RuleFor(x => x.Email)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage("Email is required")
				.Must(Email => BeAValidEmail(Email))
				.WithMessage("Invalid email address.");

			RuleFor(x => x.VATNumber)
				.Must(VAT => string.IsNullOrEmpty(VAT) || VAT.Length == 10)
				.WithMessage("VAT number must have 10 digits.");

			RuleFor(x => x.CurrencyId)
				.GreaterThan(0)
				.WithMessage("Currency is required.");

			RuleFor(x => x.CountryId)
				.GreaterThan(0)
				.WithMessage("Country is required.");

			RuleFor(x => x.BusinessSectorId)
				.GreaterThan(0)
				.WithMessage("Business sector is required.");
		}

		public bool BeAValidEmail (string? email)
		{
			if (string.IsNullOrEmpty(email))
				return false;
			var regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
			return System.Text.RegularExpressions.Regex.IsMatch(email, regex);
		}

		public bool BeAValidPhoneNumber (string? phoneNumber)
		{
			if (string.IsNullOrEmpty(phoneNumber))
				return false;
			var regex = @"^\+?[1-9]\d{1,14}$";
			return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, regex);
		}
	}
}
