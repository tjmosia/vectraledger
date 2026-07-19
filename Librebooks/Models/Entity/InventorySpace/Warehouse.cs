using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(Warehouse))]
public class Warehouse () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual string? Code { get; set; }
	public virtual string? Name { get; set; }
	public virtual int CompanyId { get; set; }

	public Company? Company { get; set; }
	public ICollection<Inventory>? Inventory { get; set; }
	public ICollection<WarehouseZone>? Zones { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Warehouse>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id).UseIdentityColumn();
			entity.Property(e => e.Name).IsRequired().HasMaxLength(155);
			entity.Property(e => e.Code).IsRequired().HasMaxLength(20);

			entity.HasIndex(e => new { e.CompanyId, e.Id })
				.IsUnique()
				.IsClustered();

			entity.HasMany(p => p.Inventory)
				.WithOne(p => p.Warehouse)
				.HasForeignKey(p => p.WarehouseId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasMany(p => p.Zones)
				.WithOne(p => p.Warehouse)
				.HasForeignKey(p => p.WarehouseId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne<Company>()
				.WithOne()
				.HasForeignKey<Warehouse>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}

