using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.AccountingSpace;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesInvoice))]
public class SalesInvoice
{
	[Key]
	public virtual int DocumentId { get; set; }
	public virtual int DueDate { get; set; }
	public virtual bool Printed { get; set; }
	public virtual bool Posted { get; set; }
	public virtual int DebitAccountId { get; set; }

	public virtual SalesDocument? Document { get; set; }
	public virtual SalesOrderInvoice? Order { get; set; }
	public virtual ICollection<SalesInvoiceCredit>? Credits { get; set; }
	public virtual ICollection<SalesInvoiceReceipt>? Receipts { get; set; }
	public virtual ICollection<SalesInvoiceWriteoff>? WriteOffs { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesInvoice>(options =>
		{
			options.HasOne(p => p.Document)
				.WithOne()
				.HasForeignKey<SalesInvoice>(p => p.DocumentId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			options.HasOne(p => p.Order)
				.WithOne(p => p.Invoice)
				.HasForeignKey<SalesOrderInvoice>(p => p.InvoiceId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Credits)
				.WithOne(p => p.Invoice)
				.HasForeignKey(p => p.InvoiceId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Receipts)
				.WithOne(p => p.Invoice)
				.HasForeignKey(p => p.InvoiceId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.WriteOffs)
				.WithOne(p => p.Invoice)
				.HasForeignKey(p => p.InvoiceId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne<LedgerAccount>()
				.WithMany()
				.HasForeignKey(p => p.DebitAccountId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}