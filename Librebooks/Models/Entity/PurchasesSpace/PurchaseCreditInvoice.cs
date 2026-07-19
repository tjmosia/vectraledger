using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.PurchasesSpace;

[Table(nameof(PurchaseCreditInvoice))]
public class PurchaseCreditInvoice
{
    public virtual int CreditId { get; set; }
    public virtual int InvoiceId { get; set; }
    public virtual decimal Amount { get; set;  }

    public PurchaseDocument? Credit { get; set; }
    public PurchaseDocument? Invoice { get; set;  }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseCreditInvoice>(entity =>
        {
            entity.HasKey(x => new {x.CreditId, x.InvoiceId})
                .IsClustered();
            entity.Property(p => p.Amount).HasColumnType(ColumnTypes.MONETARY);

		});
    }
}
