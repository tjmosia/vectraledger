using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Core.Constants;
using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryAdjustment))]
public class InventoryAdjustment () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual DateOnly Date { get; set; }
	public virtual string? Number { get; set; }
    public virtual string? Description { get; set; }
	public virtual bool Posted { get; set; }
    public virtual int CompanyId { get; set; }

	public virtual Company? Company { get; set; }
    public virtual Inventory? Inventory { get; set; }
	public ICollection<InventoryAdjustmentLine>? Lines { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<InventoryAdjustment>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id }).IsClustered();
			options.Property(p => p.Description).HasMaxLength(255);
			options.Property(p => p.Number).IsRequired().HasMaxLength(50);

			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Lines)
				.WithOne(p => p.Adjustment)
				.HasForeignKey(p => p.AdjustmentId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
