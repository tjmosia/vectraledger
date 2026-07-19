using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.PurchasesSpace;
using Librebooks.Models.Entity.SalesSpace;
using Librebooks.Models.Entity.SystemSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.DocumentSpace;

[Table(nameof(DocumentCompanyInfo))]
public class DocumentCompanyInfo () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int CompanyId { get; set; }

	[Required, MaxLength(75)]
	public virtual string? CompanyName { get; set; }

	[MaxLength(155)]
	public virtual string? PhysicalAddress { get; set; }

	[MaxLength(155)]
	public virtual string? PostalAddress { get; set; }

	[MaxLength(10)]
	public virtual string? VATNumber { get; set; }
	public virtual int? LogoId { get; set; }
	public virtual DateOnly DateCreated { get; set; }
	public virtual bool Active { get; set; }

	[MaxLength(3)]
	public virtual int CurrencyId { get; set; }

	public virtual CompanyImage? Logo { get; set; }
	public virtual Currency? Currency { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<DocumentCompanyInfo>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsClustered();

			options.HasOne<Company>()
				.WithOne()
				.HasForeignKey<DocumentCompanyInfo>(p => p.CompanyId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Logo)
				.WithMany()
				.HasForeignKey(p => p.LogoId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Currency)
				.WithOne()
				.HasForeignKey<DocumentCompanyInfo>(p => p.CurrencyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany<SalesDocument>()
				.WithOne(p => p.CompanyInfo)
				.HasForeignKey(p => p.CompanyInfoId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany<PurchaseDocument>()
				.WithOne(p => p.CompanyInfo)
				.HasForeignKey(p => p.CompanyInfoId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
