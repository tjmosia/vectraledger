using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(WarehouseRow))]
public class WarehouseRow () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual string? Code { get; set; }
	public virtual string? Name { get; set; }
	public virtual int ZoneId { get; set; }
	public virtual int CompanyId { get; set; }

	public virtual WarehouseZone? Zone { get; set; }
	public ICollection<WarehouseColumn>? Bays { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<WarehouseRow>(entity =>
		{
			entity.HasKey(x => x.Id);
			entity.Property(x => x.Id).UseIdentityColumn();
			entity.Property(x => x.Name).HasMaxLength(255);
			entity.Property(x => x.Code).HasMaxLength(20).IsRequired();
			entity.HasIndex(p => new { p.CompanyId, p.ZoneId, p.Id })
				.IsClustered()
				.IsUnique();

			entity.HasMany(p => p.Bays)
				.WithOne(p => p.Row)
				.HasForeignKey(p => p.RowId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne<Company>()
				.WithOne()
				.HasForeignKey<WarehouseRow>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
