using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesQuoteOrder))]
[PrimaryKey(nameof(QuoteId), nameof(OrderId))]
public class SalesQuoteOrder
{
	public virtual int QuoteId { get; set; }
	public virtual int OrderId { get; set; }

	public virtual SalesQuote? Quote { get; set; }
	public virtual SalesOrder? Order { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesQuoteOrder>(options =>
		{
			options.HasKey( p=> new {p.QuoteId, p.OrderId}).IsClustered();
		});
	}
}
