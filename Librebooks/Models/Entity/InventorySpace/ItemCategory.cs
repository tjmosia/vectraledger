using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(ItemCategory))]
[Index(nameof(CompanyId), nameof(Name), IsUnique = true)]
public class ItemCategory(): VersionedEntityBase()
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }
    public virtual string? Name { get; set; }
    public virtual string? Description { get; set; }
    public virtual int CompanyId { get; set; }
    public virtual int? ParentId { get; set; }

    public virtual ItemCategory? Parent { get; set; }
    public virtual Company? Company { get; set; }
    public virtual ICollection<Item>? Items { get; set; }
    public virtual ICollection<ItemCategory>? SubCategories { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
        => builder.Entity<ItemCategory>(options =>
        {
            options.Property(p=>p.Name).IsRequired().HasMaxLength(155);
            options.Property(p=>p.Description).IsRequired().HasMaxLength(255);
            options.HasIndex(p => new { p.CompanyId, p.Id })
                .IsClustered();

            options.HasMany(p => p.SubCategories)
                .WithOne(p => p.Parent)
                .HasForeignKey(p => p.ParentId)
                    .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            options.HasMany(p => p.Items)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                    .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            options.HasOne(p => p.Company)
                .WithMany()
                .HasForeignKey(p => p.CompanyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
}
