using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VectraBooks.Core.Constants;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.BankingSpace;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.SystemSpace;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SalesSpace;

[Table(nameof(SalesReceipt))]
public class SalesReceipt () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual DateTime Date { get; set; }
	public virtual string? Number { get; set; }
	public virtual string? Reference { get; set; }
	public virtual decimal Amount { get; set; }
	public virtual decimal TaxAmount { get; set; }
	public virtual decimal TotalAmount { get; set; }
	public virtual decimal TaxRate { get; set; }
	public virtual int TaxId { get; set; }
	public virtual string? Description { get; set; }
	public virtual string? Comment { get; set; }
	public virtual bool Reconciled { get; set; }
	public virtual bool Posted { get; set; }
	public virtual int BankAccountId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int CustomerId { get; set; }
	public virtual int PaymentMethodId { get; set; }

	public virtual BankAccount? BankAccount { get; set; }
	public virtual PaymentMethod? PaymentMethod { get; set; }
	public virtual ICollection<SalesInvoiceReceipt>? AllocatedInvoices { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesReceipt>(options =>
		  {
			  options.Property(p => p.Amount).HasColumnType(ColumnTypes.MONETARY);
			  options.Property(p => p.TotalAmount).HasColumnType(ColumnTypes.MONETARY);
			  options.Property(p => p.TaxAmount).HasColumnType(ColumnTypes.MONETARY);
			  options.Property(p => p.TaxRate).HasColumnType(ColumnTypes.PERCENTAGE);
			  options.Property(p => p.Description).HasMaxLength(255);
			  options.Property(p => p.Number).HasMaxLength(50);
			  options.Property(p => p.Comment).HasMaxLength(255);
			  options.Property(p => p.Reference).IsRequired().HasMaxLength(75);

			  options.HasIndex(p => new { p.CompanyId, p.Id })
				  .IsClustered();

			  options.HasMany(p => p.AllocatedInvoices)
				  .WithOne(p => p.Receipt)
				  .HasForeignKey(p => p.ReceiptId)
					  .IsRequired()
				  .OnDelete(DeleteBehavior.Restrict);

			  options.HasOne<Company>()
				  .WithMany()
				  .HasForeignKey(p => p.CompanyId)
				  .IsRequired()
				  .OnDelete(DeleteBehavior.Restrict);

			  options.HasOne<Customer>()
				  .WithMany()
				  .HasForeignKey(p => p.CustomerId)
					  .IsRequired(true)
				  .OnDelete(DeleteBehavior.Restrict);
		  });
	}
}
