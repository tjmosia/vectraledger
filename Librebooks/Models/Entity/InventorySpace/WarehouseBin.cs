
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(WarehouseBin))]
public class WarehouseBin () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual string? Code { get; set; }
	public virtual string? Name { get; set; }
	public virtual int ShelveId { get; set; }
	public virtual int CompanyId { get; set; }

	public WarehouseShelve? Shelve { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<WarehouseBin>(entity =>
		{
			entity.HasKey(x => x.Id);
			entity.Property(x => x.Id).UseIdentityColumn();
			entity.Property(x => x.Code).IsRequired().HasMaxLength(20);
			entity.Property(x => x.Name).HasMaxLength(255);

			entity.HasIndex(p => new { p.CompanyId, p.ShelveId, p.Id })
				.IsClustered()
				.IsUnique();

			entity.HasOne<Company>()
				.WithOne()
				.HasForeignKey<WarehouseBin>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}