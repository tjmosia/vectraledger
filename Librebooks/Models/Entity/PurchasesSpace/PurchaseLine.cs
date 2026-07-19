using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Core.Constants;
using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.InventorySpace;
using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.PurchasesSpace;

[Table(nameof(PurchaseLine))]
public class PurchaseLine () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int Type { get; set; }
	public virtual string? Code { get; set; }
	public virtual int? LedgerAccountId { get; set; }

	[MaxLength(255)]
	public virtual string? Description { get; set; }

	[Required, MaxLength(20)]
	public virtual string? Unit { get; set; }

	[Column(TypeName = ColumnTypes.MONETARY)]
	public virtual decimal UnitCost { get; set; }

	[Column(TypeName = ColumnTypes.PERCENTAGE)]
	public virtual decimal DiscountRate { get; set; }

	[Column(TypeName = ColumnTypes.PERCENTAGE)]
	public virtual decimal TaxRate { get; set; }

    [Column(TypeName = ColumnTypes.MONETARY)]
    public virtual decimal UnitTaxAmount { get; set; }

    [Column(TypeName = ColumnTypes.MONETARY)]
    public virtual decimal SubTotal { get; set; }

    [Column(TypeName = ColumnTypes.MONETARY)]
    public virtual decimal Total { get; set; }

	public virtual int TaxId { get; set; }
	public virtual int DocumentId { get; set; }

	[MaxLength(255)]
	public virtual string? Comment { get; set; }

	public virtual ICollection<PurchaseDocumentLine>? DocumentLines { get; set; }
	public virtual CompanyTax? Tax { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<PurchaseLine>(options =>
		   {
			   options.HasMany(p => p.DocumentLines)
				   .WithOne(p => p.Line)
				   .HasForeignKey(p => p.LineId)
					   .IsRequired()
				   .OnDelete(DeleteBehavior.Restrict);

			   options.HasOne<Item>()
					.WithMany()
					.HasPrincipalKey(i => i.Code)
					.OnDelete(DeleteBehavior.Restrict);
		   });
	}
}
