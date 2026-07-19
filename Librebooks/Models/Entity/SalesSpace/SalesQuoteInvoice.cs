using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesOrderInvoice))]
public class SalesQuoteInvoice
{
	public virtual int QuoteId { get; set; }
	public virtual int InvoiceId { get; set; }

	public virtual SalesQuote? Quote { get; set; }
	public virtual SalesInvoice? Invoice { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesQuoteInvoice>(options =>
		{
			options.HasKey(p => new { p.QuoteId, p.InvoiceId })
				.IsClustered();
		});
	}
}
