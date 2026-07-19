using Librebooks.Core.Constants;
using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryTransfer))]
public class InventoryTransfer() : VersionedEntityBase()
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }
    public virtual DateTime DateCreated { get; set; }
    public virtual DateTime DateApproved { get; set;  }
    public virtual DateTime DateReceived { get; set;  }

    [Column(TypeName = ColumnTypes.NUMBER)]
    public virtual decimal Quantity { get; set;  }

    public virtual int SourceWarehouseId { get; set; }
    public virtual int DestinationWarehouseId { get; set; }
    public virtual int InventoryId { get; set; }
    public virtual int CompanyId { get; set; }
    public virtual bool Approved { get; set;  }
    public virtual bool Received { get; set;  }

    [MaxLength(155)]
    public virtual string? RequestedBy { get; set; }

    [MaxLength(155)]
    public virtual string? ApprovedBy { get; set; }

    [MaxLength(155)]
    public virtual string? Receivedby { get; set; }

    public Inventory? Inventory { get; set; }
    public Warehouse? SourceWarehouse { get; set;  }
    public Warehouse? DestinationWarehouse { get; set;  }

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ItemDetail>(options =>
        {
            options.HasOne(p => p.Item)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.ItemId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
