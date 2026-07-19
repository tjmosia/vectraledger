using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.DocumentSpace;
using VectraBooks.Models.Entity.InventorySpace;
using VectraBooks.Models.Entity.SupplierSpace;
using VectraBooks.Models.Entity.SystemSpace;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CompanySpace;

[Table(nameof(Company))]
public class Company : VersionedEntityBase
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[MaxLength(155)]
	public virtual string SecurityId { get; set; }

	[Required, MaxLength(100)]
	public virtual string? LegalName { get; set; }

	[MaxLength(100)]
	public virtual string? TradingName { get; set; }

	[MaxLength(20)]
	public virtual string? RegNumber { get; set; }

	[MaxLength(15)]
	public virtual string? VATNumber { get; set; }

	[MaxLength(155)]
	public virtual string? PostalAddress { get; set; }

	[MaxLength(155)]
	public virtual string? PhysicalAddress { get; set; }

	[Required, MaxLength(15)]
	public virtual string? PhoneNumber { get; set; }

	[Required, MaxLength(100)]
	public virtual string? EmailAddress { get; set; }

	[MaxLength(15)]
	public virtual string? FaxNumber { get; set; }

	public virtual int YearsInBusiness { get; set; }
	public virtual int BusinessSectorId { get; set; }

	public virtual BusinessSector? BusinessSector { get; set; }
	public virtual CompanyLogo? Logo { get; set; }
	public virtual CompanyRegionalSetup? RegionalSetup { get; set; }
	public virtual ICollection<CompanyTax>? Taxes { get; set; }
	public virtual CustomerSetup? CustomerSetup { get; set; }
	public virtual SupplierSetup? SupplierSetup { get; set; }
	public virtual ICollection<CompanyUser>? Users { get; set; }
	public virtual ICollection<DocumentSetup>? DocumentSetups { get; set; }
	public virtual ICollection<CompanyLedgerAccount>? ChartOfAccounts { get; set; }
	public ItemSetup? ItemSetup { get; set; }

	public Company () : base()
	{
		SecurityId = Guid.NewGuid().ToString("N").ToUpper();
	}

	public static void OnModelCreating (ModelBuilder builder)
		=> builder.Entity<Company>(options =>
		{
			options.HasOne(p => p.Logo)
				.WithOne()
				.HasForeignKey<CompanyLogo>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
}
