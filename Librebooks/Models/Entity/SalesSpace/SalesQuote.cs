using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesQuote))]
public class SalesQuote
{
	[Key]
	public virtual int DocumentId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int CustomerId { get; set; }

	public virtual SalesDocument? Document { get; set; }

	public virtual ICollection<SalesQuoteOrder>? Orders { get; set; }
	public virtual ICollection<SalesQuoteInvoice>? Invoices { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesQuote>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.CustomerId, p.DocumentId })
				.IsClustered();

			options.HasOne(p => p.Document)
				.WithOne()
				.HasForeignKey<SalesQuote>(p => p.DocumentId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			options.HasMany(p => p.Orders)
				.WithOne(p => p.Quote)
				.HasForeignKey(p => p.QuoteId)
					.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Invoices)
				.WithOne(p => p.Quote)
				.HasForeignKey(p => p.QuoteId)
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
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);


		});
	}
}
