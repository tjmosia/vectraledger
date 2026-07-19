using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SalesSpace;

[Table(nameof(SalesDeliveryInvoice))]
public class SalesDeliveryInvoice
{
	public virtual int DeliveryId { get; set; }
	public virtual int InvoiceId { get; set; }

	public SalesInvoice? Invoice { get; set; }
	public SalesDelivery? Delivery { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesDeliveryInvoice>(options =>
		{
			options.HasKey(p => new { p.DeliveryId, p.InvoiceId });

			options.HasOne(p => p.Invoice)
				.WithMany(p => p.Deliveries)
				.HasForeignKey(p => p.InvoiceId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Delivery)
				.WithMany()
				.HasForeignKey(p => p.DeliveryId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
