using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Core.Constants;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SupplierSpace;

[Table(nameof(Supplier))]
public class Supplier
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(155)]
	public virtual string? RegisteredName { get; set; }

	[MaxLength(155)]
	public virtual string? TradingName { get; set; }

	[MaxLength(50)]
	public virtual int? VendorNumber { get; set; }

	[MaxLength(10)]
	public virtual string? VATNumber { get; set; }

	[MaxLength(15), Required]
	public virtual string? Telephone { get; set; }

	[MaxLength(100)]
	public virtual string? Email { get; set; }

	[MaxLength(15)]
	public virtual string? Fax { get; set; }

	[Required, MaxLength(155)]
	public virtual string? PhysicalAddress { get; set; }

	[MaxLength(155)]
	public virtual string? PostalAddress { get; set; }

	[Column(TypeName = ColumnTypes.PERCENTAGE)]
	public virtual decimal Discount { get; set; }

	public virtual int PaymentTermId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int? CategoryId { get; set; }
	public virtual int TaxTypeId { get; set; }

	public virtual SupplierCategory? Category { get; set; }
	public virtual CompanyTax? TaxType { get; set; }

	public virtual ICollection<SupplierAdjustment>? Adjustments { get; set; }
	public virtual ICollection<SupplierContact>? Contacts { get; set; }
	public virtual ICollection<SupplierNote>? Notes { get; set; }
	public virtual ICollection<SupplierAccountsContact>? AccountsContacts { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Supplier>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsClustered()
				.IsUnique();

			options.HasOne(p => p.TaxType)
				.WithOne()
				.HasForeignKey<Supplier>(p => p.TaxTypeId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne<Company>()
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Notes)
				.WithOne()
				.HasForeignKey(p => p.SupplierId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.AccountsContacts)
				.WithOne()
				.HasForeignKey(p => p.SupplierId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Adjustments)
				.WithOne(p => p.Supplier)
				.HasForeignKey(p => p.SupplierId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Contacts)
				.WithOne()
				.HasForeignKey(p => p.SupplierId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
