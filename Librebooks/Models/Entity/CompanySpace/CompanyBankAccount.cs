using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Models.Entity.BankingSpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CompanySpace;

[Table(nameof(CompanyBankAccount))]
public class CompanyBankAccount ()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public virtual int CompanyId { get; set; }
	public virtual int BankAccountId { get; set; }
	public virtual bool Default { get; set; }

	public virtual Company? Company { get; set; }
	public virtual BankAccount? BankAccount { get; set; }

	public CompanyBankAccount (int companyId, int bankAccountId)
		: this()
	{
		CompanyId = companyId;
		BankAccountId = bankAccountId;
	}

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CompanyBankAccount>(options =>
		   {
			   options.HasOne(p => p.Company)
				.WithOne()
				.HasForeignKey<CompanyBankAccount>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		   });
	}
}
