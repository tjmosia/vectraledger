using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(InventoryRevaluation))]
public class InventoryRevaluation(): VersionedEntityBase()
{
    public virtual int Id { get; set; }
    public virtual DateOnly Date { get; set; }
    public virtual string? Number { get; set; }
    public virtual string? Description { get; set; }
    public virtual bool Posted { get; set; }
    public virtual int CompanyId { get; set; }

    public virtual Company? Company { get; set; }

    public ICollection<InventoryRevaluationLine>? Lines { get; set; }

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InventoryRevaluation>(options =>
        {
            options.HasIndex(p => new { p.CompanyId, p.Date, p.Id }).IsClustered();
            options.Property(p => p.Id).UseIdentityColumn();
            options.Property(p => p.Number).IsRequired().HasMaxLength(50);
            options.Property(p => p.Posted).IsRequired();
            options.Property(p => p.Description).HasMaxLength(250);
            options.HasOne(p=>p.Company)
                .WithMany()
                .HasForeignKey(p=>p.CompanyId).IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            options.HasMany(p => p.Lines)
                .WithOne(p => p.Revaluation)
                .HasForeignKey(p => p.RevaluationId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
