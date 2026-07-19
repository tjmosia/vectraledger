using VectraBooks.Models.Entity.SystemSpace;

namespace VectraBooks.Areas.Systems.Data;

public class TaxData (Tax tax)
{
	public readonly int Id = tax.Id;
	public readonly string? Name = tax.Name;
	public readonly decimal Rate = tax.Rate;
	public readonly bool System = tax.System;
	public readonly string? Type = tax.Type;
}

public class CountryData (Country country)
{
	public readonly int Id = country.Id;
	public readonly string? Name = country.Name;
	public readonly string? Code = country.Code;
	public readonly string? DialingCode = country.DialingCode;
}

public class BusinessSectorData (BusinessSector sector)
{
	public readonly int Id = sector.Id;
	public readonly string? Name = sector.Name;
}

public class ShippingMethodData (ShippingMethod method)
{
	public readonly int Id = method.Id;
	public readonly string? Name = method.Name;
	public readonly string? Description = method.Description;
	public readonly string? ShortName = method.ShortName;
}

public class ShippingTermData (ShippingTerm term)
{
	public readonly int Id = term.Id;
	public readonly string? Name = term.Name;
	public readonly string? Description = term.Description;
	public readonly string? ShortName = term.ShortName;
}
public class PaymentTermData (PaymentTerm term)
{
	public readonly int Id = term.Id;
	public readonly string? Name = term.Name;
}
public class PaymentMethodData (PaymentMethod method)
{
	public readonly int Id = method.Id;
	public readonly string? Name = method.Name;
	public readonly string? Description = method.Description;
}

public class DateFormatData (DateFormat format)
{
	public readonly int Id = format.Id;
	public readonly string? Format = format.Format;
}

