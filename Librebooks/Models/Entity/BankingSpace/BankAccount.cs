using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VectraBooks.Core.Constants;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.PurchasesSpace;
using VectraBooks.Models.Entity.SalesSpace;
using VectraBooks.Models.Entity.SystemSpace;

using Microsoft.EntityFrameworkCore;


namespace VectraBooks.Models.Entity.BankingSpace;

[Table(nameof(BankAccount))]
public class BankAccount () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(155)]
	public virtual string? Name { get; set; }

    [Column(TypeName = ColumnTypes.MONETARY)]
    public virtual decimal Balance { get; set; }

    [Required, MaxLength(100)]
	public virtual string? BankName { get; set; }

	[MaxLength(50), Required]
	public virtual string? AccountNumber { get; set; }

	[MaxLength(50)]
	public virtual string? BranchName { get; set; }

	[MaxLength(20)]
	public virtual string? BranchCode { get; set; }

	[MaxLength(50)]
	public virtual string? SwiftCode { get; set; }

	public virtual bool Active { get; set; }
	public virtual int? CategoryId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int PaymentMethodId { get; set; }

	public virtual BankAccountCategory? Category { get; set; }
	public virtual Company? Company { get; set; }
	public virtual PaymentMethod? PaymentMethod { get; set; }
	public virtual CompanyBankAccount? DefaultBankAccount { get; set; }
	public virtual ICollection<SalesReceipt>? SalesReceipts { get; set; }
	public virtual ICollection<PurchasePayment>? PurchaseReceipts { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<BankAccount>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsClustered();

			options.HasOne(p => p.DefaultBankAccount)
				.WithOne(p => p.BankAccount)
				.HasForeignKey<CompanyBankAccount>(p => p.BankAccountId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.SalesReceipts)
				.WithOne(p => p.BankAccount)
				.HasForeignKey(p => p.BankAccountId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.PurchaseReceipts)
				.WithOne(p => p.BankAccount)
				.HasForeignKey(p => p.BankAccountId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Category)
				.WithMany()
				.HasForeignKey(p => p.CategoryId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		});
	}
}
