using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Core.Constants;
using VectraBooks.Models.Entity.AccountingSpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsReceiptLine))]
public class GoodsReceiptLine
{
    public virtual int Id { get; set; }
    public virtual int ReceiptId { get; set; }
    public virtual int InventoryId { get; set; }
    public virtual decimal OldQuantityOnHand { get; set; }
    public virtual decimal Quantity { get; set; }
    public virtual decimal OldAverageCost { get; set; }
    public virtual decimal CostPrice { get; set; }
    public virtual decimal NewAverageCost { get; set; }
    public virtual decimal NewInventoryValue { get; set; }
	public virtual int PostingRuleId { get; set; }
	public virtual int? SourceId { get; set; }

    public PostingRule? PostingRule { get; set; }

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<GoodsReceiptLine>(options =>
        {
            options.HasKey(p => p.Id).IsClustered(false);
            options.HasIndex(p => new { p.ReceiptId, p.Id }).IsClustered();
            options.Property(p => p.Id).UseIdentityColumn();
            options.Property(p => p.Quantity).HasColumnType(ColumnTypes.NUMBER);
			options.Property(p => p.OldQuantityOnHand).HasColumnType(ColumnTypes.NUMBER);
			options.Property(p => p.CostPrice).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.OldAverageCost).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.NewAverageCost).HasColumnType(ColumnTypes.MONETARY);

            options.HasOne(p => p.PostingRule)
                .WithMany()
                .HasForeignKey(p => p.PostingRuleId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
