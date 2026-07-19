using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Models.Entity.AccountingSpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.PurchasesSpace;

[Table(nameof(PurchaseLedgerJournal))]
public class PurchaseLedgerJournal
{
	public virtual int JournalId { get; set; }
	public virtual int PurchasesLedgerId { get; set; }

	public Journal? Journal { get; set; }
	public PurchaseLedger? PurchasesLedger { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<PurchaseLedgerJournal>(options =>
		{
			options.HasKey(p => new { p.JournalId, p.PurchasesLedgerId })
				.IsClustered();

			options.HasOne(p => p.Journal)
				.WithOne()
				.HasForeignKey<PurchaseLedgerJournal>(p => p.JournalId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.PurchasesLedger)
				.WithOne(p => p.Journal)
				.HasForeignKey<PurchaseLedgerJournal>(p => p.PurchasesLedgerId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
