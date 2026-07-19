using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Core.Constants;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.AccountingSpace;

[Table(nameof(CompanyLedgerAccount))]
public class CompanyLedgerAccount
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int AccountId { get; set; }


	[Column(TypeName = ColumnTypes.MONETARY)]
	public virtual decimal Balance { get; set; }

	public Company? Company { get; set; }
	public LedgerAccount? Account { get; set; }
	public ICollection<JournalLine>? JournalLines { get; set; }
	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CompanyLedgerAccount>(options =>
		{

			options.HasOne(p => p.Company)
				.WithMany(p => p.ChartOfAccounts)
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);



		});
	}

}
