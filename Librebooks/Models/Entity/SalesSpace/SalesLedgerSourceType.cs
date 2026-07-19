using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VectraBooks.Extensions.Models;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SalesSpace;

[Table(nameof(SalesLedgerSourceType))]
public class SalesLedgerSourceType(): VersionedEntityBase()
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    [Required, MaxLength(155)]
    public virtual string? Name { get; set; }

    [Required, MaxLength(1)]
    public virtual string? TransactionType { get; set;  }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesLedgerSourceType>(entity =>
        {
            entity.HasIndex(p => new { p.Id })
                .IsClustered();

            entity.HasIndex(p => p.Name).IsUnique();
        });
    }
}
