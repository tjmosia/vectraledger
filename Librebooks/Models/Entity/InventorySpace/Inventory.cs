using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Core.Constants;
using VectraBooks.Extensions.Models;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(Inventory))]
public class Inventory () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual int ItemId { get; set; }
	public virtual decimal QuantityOnHand { get; set; }
	public virtual decimal QuantityReserved { get; set; }
    public virtual decimal MinQuantity { get; set; }
	public virtual decimal MaxQuantity { get; set; }
	public virtual decimal InventoryValue { get; set; }
	public virtual decimal AverageCost { get; set; }

    public virtual int? WarehouseId { get; set; }
	public virtual int? BayId { get; set; }
	public virtual int? ShelveId { get; set; }
	public virtual int? BinId { get; set; }
	public virtual decimal? Weight { get; set; }

	public Item? Item { get; set; }
	public Warehouse? Warehouse { get; set; }
	public ICollection<InventoryAdjustmentLine>? Adjustments { get; set; }
	public WarehouseBin? Bin { get; set; }
	public WarehouseShelve? Shelve { get; set; }
	public WarehouseColumn? Bay { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Inventory>(options =>
		{
			// Table
			options.ToTable(nameof(Inventory));

			// Key
			options.HasKey(x => x.Id);
			options.Property(x => x.Id).UseIdentityColumn();

			// Indexes
			options.HasIndex(p => new { p.ItemId, p.Id })
				.IsUnique()
				.IsClustered();

			options.Property(x => x.QuantityOnHand)
				.HasColumnType(ColumnTypes.NUMBER);

			options.Property(x => x.QuantityReserved)
				.HasColumnType(ColumnTypes.NUMBER);

			options.Property(x => x.MinQuantity)
				.HasColumnType(ColumnTypes.NUMBER);

			options.Property(x => x.MaxQuantity)
				.HasColumnType(ColumnTypes.NUMBER);

			options.Property(x => x.Weight)
				.HasColumnType(ColumnTypes.NUMBER);

			options.Property(x => x.InventoryValue)
				.HasColumnType(ColumnTypes.MONETARY);

			options.Property(x => x.AverageCost)
				.HasColumnType(ColumnTypes.MONETARY);

			options.HasOne(p => p.Shelve)
				.WithOne()
				.HasForeignKey<Inventory>(p => p.ShelveId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasOne(p => p.Bin)
				.WithOne()
				.HasForeignKey<Inventory>(p => p.ShelveId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasOne(p => p.Bay)
				.WithOne()
				.HasForeignKey<Inventory>(p => p.BayId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);
		});
	}
}
