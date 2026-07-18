using Librebooks.Core.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryRevaluationLine))]
public class InventoryRevaluationLine
{
    public virtual int Id { get; set; }
    public virtual int RevaluationId { get; set; }
    public virtual int InventoryId { get; set; }
    public virtual decimal OldAverageCost { get; set; }
    public virtual decimal OldInventoryValue { get; set; }
    public virtual decimal NewAverageCost { get; set; }
    public virtual decimal NewInventoryValue { get; set; }
    public virtual decimal QuantityOnHand { get; set; }
    public virtual int PostingRuleId { get; set;  }

    public InventoryRevaluation? Revaluation { get; set; }

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
            options.Property(p => p.NewAverageCost).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.NewInventoryValue).HasColumnType(ColumnTypes.MONETARY);
            options.Property(p => p.QuantityOnHand).HasColumnType(ColumnTypes.MONETARY);
        });
    }
}