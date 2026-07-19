using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesOrderInvoice))]
public class SalesOrderInvoice
{
    public virtual int OrderId { get; set; }
    public virtual int InvoiceId { get; set; }

    public virtual SalesOrder? Order { get; set; }
    public virtual SalesInvoice? Invoice { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
    {
        builder.Entity<SalesOrderInvoice>(options =>
        {
            options.HasKey(p=> new { p.OrderId, p.InvoiceId })
                .IsClustered();
                
            options.HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            options.HasOne(p => p.Invoice)
                .WithMany()
                .HasForeignKey (p => p.InvoiceId)
                    .IsRequired()
                .OnDelete (DeleteBehavior.Restrict);

		});
    }
}
