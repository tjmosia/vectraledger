using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(WarehouseColumn))]
public class WarehouseColumn () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(20)]
	public virtual string? Code { get; set; }

	[MaxLength(255)]
	public virtual string? Name { get; set; }

	public virtual int RowId { get; set; }
	public virtual int CompanyId { get; set; }

	public WarehouseRow? Row { get; set; }
	public ICollection<WarehouseShelve>? Shelves { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<WarehouseColumn>(entity =>
		{
			entity.HasIndex(p => new { p.CompanyId, p.RowId, p.Id })
				.IsClustered()
				.IsUnique();

			entity.HasMany(p => p.Shelves)
				.WithOne(p => p.Column)
				.HasForeignKey(p => p.ColumnId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne<Company>()
				.WithOne()
				.HasForeignKey<WarehouseColumn>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}