using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CustomerSpace;

[Table(nameof(CustomerCategory))]
public class CustomerCategory () : VersionedEntityBase()
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    [Required, MaxLength(75)]
    public virtual string? Name { get; set; }

    [MaxLength(255)]
    public virtual string? Description { get; set; }

    public virtual int CompanyId { get; set; }

    public virtual ICollection<Customer>? Customers { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
    {
        builder.Entity<CustomerCategory>(options =>
        {
            options.HasIndex(p => new { p.CompanyId, p.Id })
                .IsClustered();

            options.HasMany(p => p.Customers)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                    .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            options.HasOne<Company>()
                .WithMany()
                .HasForeignKey(p => p.CompanyId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
