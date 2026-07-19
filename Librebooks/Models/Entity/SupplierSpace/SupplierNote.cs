using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Models.Entity.GeneralSpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SupplierSpace;

[Table(nameof(SupplierNote))]
public class SupplierNote
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public virtual int NoteId { get; set; }
    public virtual int SupplierId { get; set; }

    public virtual Note? Note { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
    {
        builder.Entity<SupplierNote>(options =>
        {
            options.ToTable(nameof(SupplierNote))
                .HasKey(p => new { p.SupplierId, p.NoteId })
                .IsClustered();
        });
    }
}