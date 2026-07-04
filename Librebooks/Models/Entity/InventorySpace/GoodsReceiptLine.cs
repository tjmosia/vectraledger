using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsReceiptLine))]
public class GoodsReceiptLine
{
    public virtual int Id { get; set; }
    public virtual int ReceiptId { get; set; }


    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoodsReceiptLine>(entity =>
        {

        });
    }
}
