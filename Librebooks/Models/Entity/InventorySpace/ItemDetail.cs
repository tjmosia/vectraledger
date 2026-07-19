using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(ItemDetail))]
public class ItemDetail(): VersionedEntityBase()
{
    public virtual int Id { get; set; }
    public virtual string? Name { get; set; }
    public virtual string? Value { get; set; }
    public virtual int ItemId { get; set; }

	public virtual Item? Item { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
    {
        builder.Entity<ItemDetail>(options =>
        {
            options.HasKey(e => e.Id);
            options.Property(p => p.Id).UseIdentityColumn();
            options.Property(p => p.Name).IsRequired().HasMaxLength(75);
            options.Property(p => p.Value).IsRequired().HasMaxLength(155);
            options.HasIndex(p => new { p.ItemId, p.Id })
				.IsClustered()
				.IsUnique();

			options.HasOne(p => p.Item)
                .WithMany(p => p.Details)
                .HasForeignKey(p => p.ItemId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
