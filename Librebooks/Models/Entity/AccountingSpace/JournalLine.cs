using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.AccountingSpace;

[Table(nameof(JournalLine))]
public class JournalLine
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual decimal Debit { get; set; }
	public virtual decimal Credit { get; set; }
	public virtual int JournalId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int LedgerAccountId { get; set; }
	public virtual int? SourceLineId { get; set; }

	public virtual Company? Company { get; set; }
	public virtual Journal? Journal { get; set; }
	public virtual CompanyLedgerAccount? LedgerAccount { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<JournalLine>(options =>
		{
			options.HasKey(p => p.Id);
			options.Property(p => p.Id).UseIdentityColumn();
			options.Property(p => p.Credit).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.Debit).HasColumnType(ColumnTypes.MONETARY);

			options.HasIndex(p => new { p.CompanyId, p.LedgerAccountId, p.JournalId, p.Id })
				.IsClustered();

			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.LedgerAccount)
				.WithMany(p => p.JournalLines)
				.HasForeignKey(p => p.LedgerAccountId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
