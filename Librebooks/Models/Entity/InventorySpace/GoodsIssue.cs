using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.DocumentSpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsIssue))]
public class GoodsIssue(): VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual DateTime Date { get; set;  }
	public virtual DateTime DateApproved { get; set; }
	public virtual string? Number { get; set; }
    public virtual string? Reference { get; set; }
    public virtual string? Description { get; set;  }
	public virtual int? SourceId { get; set; }
	public virtual string? SourceType { get; set; }
    public virtual int CompanyId { get; set; }
	public virtual bool Recorded { get; set; }
	public virtual int WarehouseId { get; set; }
	public virtual int StatusId { get; set; }

	public DocumentStatus? Status { get; set; }
	public Company? Company { get; set; }

	public ICollection<GoodsIssueLine>? Items { get; set; }

	public static void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<GoodsIssue>( options =>
		{
            options.HasIndex(p => new { p.CompanyId, p.Id }).IsClustered();
			options.Property(p => p.Description).HasMaxLength(255);
			options.Property(p => p.Reference).HasMaxLength(20);
			options.Property(p => p.Number).HasMaxLength(75);

			options.HasOne(p => p.Status)
				.WithMany()
				.HasForeignKey(p => p.StatusId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
