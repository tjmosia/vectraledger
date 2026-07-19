using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Core.Constants;
using Librebooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(Inventory))]
public class Inventory () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	public virtual int ItemId { get; set; }

	[Column(TypeName = ColumnTypes.NUMBER)]
	public virtual decimal QuantityOnHand { get; set; }

	[Column(TypeName = ColumnTypes.NUMBER)]
	public virtual decimal MinQuantity { get; set; }

	[Column(TypeName = ColumnTypes.NUMBER)]
	public virtual decimal MaxQuantity { get; set; }

	public virtual int? WarehouseId { get; set;  }
	public virtual int? ShelveId{ get; set;  }
	public virtual int? BinId { get; set;  }

	[Column(TypeName = ColumnTypes.NUMBER)]
	public virtual decimal? Weight { get; set;  }

	public Item? Item { get; set; }
	public Warehouse? Warehouse { get; set; }
	public InventoryAdjustment? Adjustments { get; set;  }
	public WarehouseBin? Bin { get; set; }
	public WarehouseShelve? Shelve { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Inventory>(options =>
		{
			options.HasIndex(p => new { p.ItemId, p.Id })
				.IsUnique()
				.IsClustered();

			options.HasOne(p => p.Adjustments)
				.WithOne(p => p.Inventory)
				.HasForeignKey<InventoryAdjustment>(p => p.InventoryId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p=>p.Shelve)
				.WithOne()
				.HasForeignKey<Inventory>(p=>p.ShelveId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasOne(p=>p.Bin)
				.WithOne()
				.HasForeignKey<Inventory>(p=>p.ShelveId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);
		});
	}
}
