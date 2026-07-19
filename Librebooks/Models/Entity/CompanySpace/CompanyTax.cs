using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.InventorySpace;
using VectraBooks.Models.Entity.PurchasesSpace;
using VectraBooks.Models.Entity.SalesSpace;
using VectraBooks.Models.Entity.SystemSpace;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CompanySpace;

[Table(nameof(CompanyTax))]
public class CompanyTax ()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int TaxId { get; set; }
	public virtual bool Default { get; set; } = false;

	public CompanyTax (int companyId, int taxTypeId)
		: this()
	{
		CompanyId = companyId;
		TaxId = taxTypeId;
	}

	public virtual Tax? Tax { get; set; }
	public virtual Company? Company { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CompanyTax>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.TaxId, p.Id })
				.IsUnique()
				.IsClustered();

			options.HasOne(p => p.Company)
				.WithMany(p => p.Taxes)
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			options.HasOne(p => p.Tax)
				.WithOne()
				.HasForeignKey<CompanyTax>(p => p.TaxId)
				.IsRequired(true)
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany<JournalLine>()
				.WithOne(p => p.Tax)
				.HasForeignKey(p => p.TaxId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasMany<LedgerAccount>()
				.WithOne(p => p.Tax)
				.HasForeignKey(p => p.TaxId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasMany<SalesDocumentLine>()
				.WithOne(p => p.Tax)
				.HasForeignKey(p => p.TaxId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasMany<PurchaseLine>()
				.WithOne(p => p.Tax)
				.HasForeignKey(p => p.TaxId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasMany<Item>()
				.WithOne(p => p.TaxType)
				.HasForeignKey(p => p.TaxId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);
		});
	}
}
