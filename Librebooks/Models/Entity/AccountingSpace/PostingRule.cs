using Librebooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.AccountingSpace;

[Table(nameof(PostingRule))]
public class PostingRule
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual string? Group { get; set;  }
	public virtual string? Reason { get; set;  }
	public virtual int CreditAccountId { get; set; }
	public virtual int DebitAccountId { get; set; }
	public virtual int CompanyId { get; set; }

	public LedgerAccount? DebitAccount { get; set; }
	public LedgerAccount? CreditAccount { get; set;  }

	public static void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PostingRule>( entity =>
		{
			entity.HasIndex(p => new { p.CompanyId, p.Id }).IsClustered();
			entity.HasIndex(p => new { p.Reason, p.DebitAccountId, p.CreditAccountId }).IsUnique();
			entity.Property(p => p.Reason).HasMaxLength(155);
			entity.Property(p => p.Group).HasMaxLength(155);
			entity.HasOne<Company>()
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
				
		});
	}
}
