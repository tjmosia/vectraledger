using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.InventorySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesDocumentLine))]
public class SalesDocumentLine
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int DocumentId { get; set; }
	public virtual bool ItemType { get; set; }
	public virtual int? ParentItemId { get; set; }
	public virtual int? ItemId { get; set; }
	public virtual string? ItemCode { get; set; }
	public virtual int DebitAccountId { get; set; }
	public virtual int CreditAccountId { get; set; }
	public virtual string? Description { get; set; }
	public virtual string? Unit { get; set; }
	public virtual decimal QuantityOrdered { get; set; }
	public virtual decimal? QuantityInvoiced { get; set; }
	public virtual decimal Price { get; set; }
	public virtual decimal DiscountRate { get; set; }
	public virtual decimal TaxRate { get; set; }
	public virtual decimal TaxAmount { get; set; }
	public virtual decimal SubTotal { get; set; }
	public virtual int TaxId { get; set; }
	public virtual string? Comment { get; set; }
	public virtual int SourceLineId { get; set; }

	
	public decimal GetBackOrderQuantity () { 
		if (QuantityInvoiced == null)
			return 0M;
		return QuantityOrdered - QuantityInvoiced.Value;
		
	}

	public virtual ICollection<SalesDocumentLine>? DocumentLines { get; set; }
	public virtual Item? Item { get; set; }
	public virtual CompanyTax? Tax { get; set; }
	public virtual SalesDocument? Document { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesLine>(options =>
		{
			options.Property(p => p.Item).HasMaxLength(50);
			options.Property(p => p.Description).HasMaxLength(50).IsRequired();
			options.Property(p => p.Unit).HasMaxLength(20);
			options.Property(p => p.Comment).HasMaxLength(400);
			options.Property(p => p.TaxRate).HasColumnType(ColumnTypes.PERCENTAGE);
			options.Property(p => p.DiscountRate).HasColumnType(ColumnTypes.PERCENTAGE);
			options.Property(p => p.Price).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.TaxAmount).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.SubTotal).HasColumnType(ColumnTypes.MONETARY);

			options.HasOne<Item>()
				 .WithMany()
				 .HasPrincipalKey(i => i.Code)
				 .OnDelete(DeleteBehavior.Restrict);
		});
	}
}
