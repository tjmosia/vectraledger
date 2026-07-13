using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(WarehouseShelve))]
public class WarehouseShelve() : VersionedEntityBase()
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    [Required, MaxLength(20)]
    public virtual string? Code { get; set; }

    [MaxLength(255)]
    public virtual string? Name { get; set; }

    public virtual int ColumnId { get; set; }
    public virtual int CompanyId { get; set; }

    public WarehouseColumn? Column { get; set; }
    public ICollection<WarehouseBin>? Bins { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WarehouseShelve>(entity =>
        {
            entity.HasIndex(p => new {p.CompanyId, p.ColumnId, p.Id })
                .IsClustered()
                .IsUnique();

            entity.HasMany(p => p.Bins)
                .WithOne(p => p.Shelve)
                .HasForeignKey(p => p.ShelveId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Company>()
                .WithOne()
                .HasForeignKey<WarehouseShelve>(p => p.CompanyId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}