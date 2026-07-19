using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Core.Constants;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryTransferLine))]
public class InventoryTransferLine
{
	public virtual int Id { get; set; }
	public virtual int InventoryTransferId { get; set; }
	public virtual int InventoryId { get; set; }
	public virtual string? UOM { get; set; }
	public virtual decimal Quantity { get; set; }
	public virtual decimal OldQtyOnHandAtSourceWarehouse { get; set; }
	public virtual decimal OldQtyOnHandAtDestinationWarehouse { get; set; }
	public virtual string? Note { get; set;  }

	public InventoryTransfer? InventoryTransfer { get; set; }
	public Inventory? Inventory { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<InventoryTransferLine>(options =>
		{
			options.HasKey(e => e.Id);
			options.Property(p => p.Id).UseIdentityColumn();
			options.HasIndex(p => new { p.InventoryTransferId, p.Id }).IsClustered();
			options.Property(p=>p.UOM).HasMaxLength(50).IsRequired();
			options.Property(p => p.Quantity).HasColumnType(ColumnTypes.NUMBER);
			options.Property(p => p.OldQtyOnHandAtSourceWarehouse).HasColumnType(ColumnTypes.NUMBER);
			options.Property(p => p.OldQtyOnHandAtDestinationWarehouse).HasColumnType(ColumnTypes.NUMBER);
			options.Property(p => p.Note).HasMaxLength(500);

			options.HasOne(p=>p.Inventory)
				.WithMany()
				.HasForeignKey(p => p.InventoryId)
				.OnDelete(DeleteBehavior.Restrict);

		});
	}
}