using Librebooks.Core.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsIssueItem))]
public class GoodsIssueItem
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }
    public virtual int IssueId { get; set;  }
    public virtual int ItemId { get; set; }
    public virtual string? Unit { get; set; }
    public virtual string? Code { get; set; }
    public virtual string? Description { get; set;  }
    public virtual decimal Quantity { get; set;  }
    public virtual int WarehouseId { get; set; }
    public virtual int? SourceLineId { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoodsIssueItem>( entity =>
        {
            entity.Property(p=>p.Unit).HasMaxLength(20);
            entity.Property(p=>p.Description).HasMaxLength(20).IsRequired();
            entity.Property(p=>p.Code).HasMaxLength(20);
            entity.Property(p => p.Quantity).HasColumnType(ColumnTypes.NUMBER);

        });
    }
}
