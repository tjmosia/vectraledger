using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.PurchasesSpace
{
    [Table(nameof(PurchaseOrderInvoice))]
    public class PurchaseOrderInvoice
    {
        public virtual int OrderId { get; set; }
        public virtual int InvoiceId { get; set; }

        public virtual PurchaseDocument? Order { get; set; }
        public virtual PurchaseDocument? Invoice { get; set; }

        public static void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<PurchaseOrderInvoice>(options =>
            {
                options.HasKey(p => new { p.OrderId, p.InvoiceId })
                    .IsClustered();
            });
        }
    }
}