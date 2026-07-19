using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Core.Constants;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.DocumentSpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryAdjustment))]
public class InventoryAdjustment () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual DateOnly Date { get; set; }
	public virtual string? Number { get; set; }
	public virtual bool Posted { get; set; }
    public virtual int CompanyId { get; set; }
    public virtual int StatusId { get; set; }
	public virtual string? Message { get; set; }

	public DocumentStatus? Status { get; set; }
	public Company? Company { get; set; }
	public ICollection<InventoryAdjustmentLine>? Lines { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<InventoryAdjustment>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id }).IsClustered();
			options.Property(p => p.Message).HasMaxLength(255);
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

			options.HasOne(p => p.Status)
				.WithMany()
				.HasForeignKey(p => p.StatusId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
