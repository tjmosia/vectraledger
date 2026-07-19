using VectraBooks.Models.Entity.CompanySpace;

namespace VectraBooks.Areas.Companies.Data;

public readonly struct CompanySummaryData (Company company)
{
	public readonly int CompanyId = company.Id;
	public readonly string Name = company.LegalName!;
	public readonly string? Logo = company.Logo?.Image!.PathName;
}
