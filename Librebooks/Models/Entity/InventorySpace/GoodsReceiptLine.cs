using Librebooks.Core.Constants;
using Librebooks.Models.Entity.AccountingSpace;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsReceiptLine))]
public class GoodsReceiptLine
{
    public virtual int Id { get; set; }
    public virtual int ReceiptId { get; set; }
    public virtual int InventoryId { get; set; }
    public virtual decimal OldQuantityOnHand { get; set; }
    public virtual decimal Quantity { get; set; }
    public virtual decimal OldAverageCost { get; set; }
    public virtual decimal AverageCost { get; set; }
    public virtual int PostingRuleId { get; set; }  

    public PostingRule? PostingRule { get; set; }

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<GoodsReceiptLine>(options =>
        {
            options.HasKey(p => p.Id).IsClustered(false);
            options.HasIndex(p => new { p.ReceiptId, p.Id }).IsClustered();
            options.Property(p => p.Id).UseIdentityColumn();
            options.Property(p => p.Quantity).HasColumnType(ColumnTypes.NUMBER);
            options.Property(p => p.AverageCost).HasColumnType(ColumnTypes.NUMBER);
            options.Property(p => p.OldQuantityOnHand).HasColumnType(ColumnTypes.NUMBER);
            options.Property(p => p.OldAverageCost).HasColumnType(ColumnTypes.NUMBER);
        });
    }
}
