using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Core.Constants;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.PurchasesSpace
{
    [Table(nameof(PurchaseDocumentLine))]
    public class PurchaseDocumentLine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int DocumentId { get; set; }
        public virtual int LineId { get; set; }
        public virtual decimal Quantity { get; set; }

        public virtual PurchaseLine? Line { get; set; }
        public virtual PurchaseDocument? Document { get; set; }

        public static void OnModelCreating (ModelBuilder builder)
            => builder.Entity<PurchaseDocumentLine>(options =>
            {
                options.ToTable(nameof(PurchaseDocumentLine))
                    .HasKey(e => new { e.DocumentId, e.LineId })
                    .IsClustered(true);

                options.Property(p => p.Quantity)
                    .HasColumnType(ColumnTypes.NUMBER);
            });
    }
}
