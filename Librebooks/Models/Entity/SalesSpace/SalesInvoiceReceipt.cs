using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesInvoiceReceipt))]
[PrimaryKey(nameof(InvoiceId), nameof(ReceiptId))]
public class SalesInvoiceReceipt
{
    public virtual int InvoiceId { get; set; }
    public virtual int ReceiptId { get; set; }

    [Column(TypeName = ColumnTypes.MONETARY)]
    public virtual decimal Amount { get; set; }

    public virtual SalesInvoice? Invoice { get; set; }
    public virtual SalesReceipt? Receipt { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
    {
        builder.Entity<SalesInvoiceReceipt>(options =>
        {

        });
    }
}
