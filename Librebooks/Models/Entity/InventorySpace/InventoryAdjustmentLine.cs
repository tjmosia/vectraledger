using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Core.Constants;
using VectraBooks.Models.Entity.AccountingSpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryAdjustmentLine))]
public class InventoryAdjustmentLine
{
    public virtual int Id { get; set; }
    public virtual int InventoryId { get; set; }
    public virtual decimal OldQuantityOnHand { get; set; }
    public virtual decimal NewQuantityOnHand { get; set; }
    public virtual decimal AverageCost { get; set; }
    public virtual int PostingRuleId { get; set; }
    public virtual int AdjustmentId { get; set; }

    public InventoryAdjustment? Adjustment { get; set; }
    public Inventory? Inventory { get; set; }
	public PostingRule? PostingRule { get; set; }

	public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InventoryAdjustmentLine>(options =>
        {
            options.HasKey(p => p.Id);
			options.Property(p => p.Id).UseIdentityColumn();
			options.HasIndex(p => new { p.InventoryId, p.Id }).IsClustered();
            options.Property(p => p.OldQuantityOnHand).HasColumnType(ColumnTypes.NUMBER);
            options.Property(p => p.NewQuantityOnHand).HasColumnType(ColumnTypes.NUMBER);
            options.Property(p => p.AverageCost).HasColumnType(ColumnTypes.MONETARY);

            options.HasOne(p => p.Inventory)
                .WithMany()
                .HasForeignKey(p => p.InventoryId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            options.HasOne(p => p.PostingRule)
                .WithMany()
                .HasForeignKey(p => p.PostingRuleId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


		});
    }
}