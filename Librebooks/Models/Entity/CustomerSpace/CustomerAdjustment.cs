using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Models.Entity.AccountingSpace;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.SystemSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.CustomerSpace;

[Table(nameof(CustomerAdjustment))]
public class CustomerAdjustment
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual string? Number { get; set; }
	public virtual string? Reference { get; set; }
	public virtual int TaxId { get; set; }
	public virtual decimal TaxRate { get; set; }
	public virtual decimal Amount { get; set;  }
	public virtual decimal TaxAmount { get; set; }
	public virtual decimal TotalAmount { get; set; }
    public virtual int PostingRule { get; set; }
    public virtual int DebitAccountId { get; set; }
	public virtual int CreditAccountId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int CustomerId { get; set; }
    public virtual string? Description { get; set; }
    public virtual string? Comments { get; set; }
    public virtual bool Posted { get; set; }
	public virtual int JournalId { get; set; }

    public Customer? Customer { get; set; }
	public Tax? Tax { get; set; }
	public CompanyLedgerAccount? DebitAccount { get; set; }
	public CompanyLedgerAccount? CreditAccount { get; set; }
	public Journal? Journal { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CustomerAdjustment>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.CustomerId, p.Id }).IsClustered();
			options.HasIndex(p => new { p.CompanyId, p.Number }).IsUnique();
			options.Property(p => p.Amount).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.TaxAmount).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.TotalAmount).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.Description).HasMaxLength(255);
			options.Property(p => p.Comments).HasMaxLength(255);
			options.Property(p => p.Number).HasMaxLength(50);
			options.Property(p => p.Reference).HasMaxLength(75);

			options.HasOne<Company>()
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Customer)
				.WithMany()
				.HasForeignKey(p => p.CustomerId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Journal)
				.WithMany()
				.HasForeignKey(p => p.JournalId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
