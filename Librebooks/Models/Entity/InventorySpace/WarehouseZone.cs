using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(WarehouseZone))]
public class WarehouseZone () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(20)]
	public virtual string? Code { get; set; }

	[MaxLength(255)]
	public virtual string? Name { get; set; }

	public virtual int WarehouseId { get; set; }
	public virtual int CompanyId { get; set; }

	public Warehouse? Warehouse { get; set; }
	public ICollection<WarehouseRow>? Rows { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<WarehouseZone>(entity =>
		{
			entity.HasIndex(p => new { p.CompanyId, p.WarehouseId, p.Id })
				.IsClustered()
				.IsUnique();

			entity.HasMany(p => p.Rows)
				.WithOne(p => p.Zone)
				.HasForeignKey(p => p.ZoneId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne<Company>()
				.WithOne()
				.HasForeignKey<WarehouseZone>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
