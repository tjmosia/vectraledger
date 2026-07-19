using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.AccountingSpace;

[Table(nameof(PostingRule))]
public class PostingRule(): VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int GroupId { get; set;  }
	public virtual string? Reason { get; set;  }
	public virtual int CreditAccountId { get; set; }
	public virtual int DebitAccountId { get; set; }

	public LedgerAccount? DebitAccount { get; set; }
	public LedgerAccount? CreditAccount { get; set;  }
	public PostingRuleGroup? Group { get; set; }

	public static void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PostingRule>( entity =>
		{
			entity.HasIndex(p => new { p.DebitAccountId, p.CreditAccountId, p.Reason, }).IsUnique();
			entity.Property(p => p.Group).IsRequired().HasMaxLength(155);
					
		});
	}
}
