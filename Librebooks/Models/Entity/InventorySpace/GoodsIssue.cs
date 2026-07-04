using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsIssue))]
public class GoodsIssue
{
	public virtual int Id { get; set; }
	public virtual DateTime Date { get; set;  }
	public virtual DateTime DateApproved { get; set; }
	public virtual string? Number { get; set; }
	public virtual string? Description { get; set;  }
	public virtual string? Reference { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual bool Recorded { get; set;  }

    public ICollection<GoodsIssueItem>? Items { get; set; }

	public static void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<GoodsIssue>( options =>
		{
			options.HasKey( x => x.Id );
            options.Property(x => x.Id).UseIdentityColumn();
            options.HasIndex(p => new { p.CompanyId, p.Id }).IsClustered();

			options.Property(p => p.Description).HasMaxLength(255);
		});
	}
}
