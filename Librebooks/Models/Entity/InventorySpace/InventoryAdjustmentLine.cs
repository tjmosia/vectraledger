using Librebooks.Core.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryAdjustmentLine))]
public class InventoryAdjustmentLine
{
    public virtual int Id   { get; set; }
    public virtual int InventoryId { get; set; }
    public virtual decimal OldQuantityOnHand { get; set; }
    public virtual decimal NewQuantityOnHand { get; set; }
    public virtual decimal AverageCost { get; set; }
    public virtual int PostingRuleId { get; set; }
    public virtual int AdjustmentId { get; set; }

    public InventoryAdjustment? Adjustment { get; set; }

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InventoryAdjustmentLine>(options =>
        {
            options.HasKey(p => p.Id).IsClustered(false);
            options.HasIndex(p => new { p.InventoryId, p.Id }).IsClustered();
            options.Property(p => p.Id).UseIdentityColumn();
            options.Property(p => p.OldQuantityOnHand).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.NewQuantityOnHand).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.AverageCost).HasColumnType(ColumnTypes.MONETARY);
        });
    }
}