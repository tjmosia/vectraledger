using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.DocumentSpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryTransfer))]
public class InventoryTransfer() : VersionedEntityBase()
{
    public virtual int Id { get; set; }
    public virtual DateTime DateCreated { get; set; }
    public virtual DateTime DateReceived { get; set;  }
    public virtual string? Number { get; set; }
	public virtual decimal Quantity { get; set;  }
    public virtual string? Message { get; set; }
	public virtual int WarehouseId { get; set; }
    public virtual int DestinationWarehouseId { get; set; }
    public virtual int CompanyId { get; set; }
    public virtual bool Approved { get; set;  }
    public virtual bool Received { get; set;  }
    public virtual string? RequestedBy { get; set; }
    public virtual string? ReleasedBy { get; set; }
	public virtual bool Posted { get; set; }
    public virtual int StatusId { get; set; }

    public DocumentStatus? Status { get; set; }
	public Company? Company { get; set;  }
	public Warehouse? Warehouse { get; set;  }
    public Warehouse? DestinationWarehouse { get; set;  }
    public ICollection<InventoryTransferLine>? Lines { get; set; }

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InventoryTransfer>(options =>
        {
            options.HasKey(x => x.Id);
            options.HasIndex(p=> new {p.CompanyId, p.Id}).IsClustered();
            options.Property(p => p.Id).UseIdentityColumn();
            options.HasIndex(p=>new {p.CompanyId, p.Number}).IsUnique();
            options.Property(p => p.Message).HasMaxLength(500);
            options.Property(p => p.Number).IsRequired().HasMaxLength(50);

			options.HasMany(p=>p.Lines)
                .WithOne(l=>l.InventoryTransfer)
                .HasForeignKey(l=>l.InventoryTransferId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            options.HasOne(p =>p.Warehouse)
                .WithMany()
                .HasForeignKey(p => p.WarehouseId)
					.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            options.HasOne(p =>p.DestinationWarehouse)
                .WithMany()
                .HasForeignKey(p => p.DestinationWarehouseId)
					.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            options.HasOne(p =>p.Company)
                .WithMany()
                .HasForeignKey(p => p.CompanyId)
					.IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

		});
    }
}