using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;

namespace VectraBooks.Models.Entity.AccountingSpace;

[Table(nameof(PostingRule))]
public class PostingRule(): VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual int GroupId { get; set;  }
	public virtual string? Reason { get; set;  }
	public virtual string? ReasonCode { get; set; }
	public virtual int CreditAccountId { get; set; }
	public virtual int DebitAccountId { get; set; }

	public LedgerAccount? DebitAccount { get; set; }
	public LedgerAccount? CreditAccount { get; set;  }
	public PostingRuleGroup? Group { get; set; }

	public static void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PostingRule>(entity =>
		{
			entity.HasKey(x => x.Id);
			entity.Property(p => p.Id).UseIdentityColumn();
			entity.HasIndex(p => new { p.DebitAccountId, p.CreditAccountId, p.Reason, }).IsUnique();
			entity.Property(p => p.Reason).IsRequired().HasMaxLength(155);
			entity.Property(p => p.ReasonCode).IsRequired().HasMaxLength(155);

			entity.HasOne(p => p.DebitAccount)
				.WithMany()
				.HasForeignKey(p => p.DebitAccountId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(p => p.CreditAccount)
				.WithMany()
				.HasForeignKey(p => p.CreditAccountId)
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
