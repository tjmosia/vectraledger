using System.ComponentModel.DataAnnotations.Schema;

using VectraBooks.Core.Constants;
using VectraBooks.Models.Entity.AccountingSpace;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryRevaluationLine))]
public class InventoryRevaluationLine
{
    public virtual int Id { get; set; }
    public virtual int RevaluationId { get; set; }
    public virtual int InventoryId { get; set; }
    public virtual decimal OldAverageCost { get; set; }
    public virtual decimal OldInventoryValue { get; set; }
    public virtual decimal AverageCost { get; set; }
    public virtual decimal InventoryValue { get; set; }
    public virtual decimal QuantityOnHand { get; set; }
    public virtual int PostingRuleId { get; set;  }

    public InventoryRevaluation? Revaluation { get; set; }
    public Inventory? Inventory { get; set; }
    public PostingRule? PostingRule { get; set; }

	public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InventoryRevaluationLine>(options =>
        {
            options.HasKey(p => p.Id).IsClustered(false);
            options.HasIndex(p=> new {p.RevaluationId, p.Id}).IsClustered();
            options.Property(p => p.Id).UseIdentityColumn();
            options.HasIndex(p => new { p.RevaluationId, p.InventoryId, p.Id }).IsClustered();
            options.Property(p => p.OldAverageCost).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.OldInventoryValue).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.AverageCost).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.InventoryValue).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.QuantityOnHand).HasColumnType(ColumnTypes.NUMBER);

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