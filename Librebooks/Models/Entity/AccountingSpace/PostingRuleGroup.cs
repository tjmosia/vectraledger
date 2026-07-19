using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Extensions.Models;

namespace VectraBooks.Models.Entity.AccountingSpace;

[Table(nameof(PostingRuleGroup))]
public class PostingRuleGroup():VersionedEntityBase()
{
    public virtual int Id { get; set; }
    public virtual string? Name { get; set;  }

    public ICollection<PostingRule>? Rules { get; set; }    

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostingRuleGroup>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(p => p.Id).UseIdentityColumn();
			entity.HasIndex(p => p.Name).IsUnique();
            entity.Property(p => p.Name).IsRequired().HasMaxLength(75);

            entity.HasMany(p => p.Rules)
                .WithOne(p => p.Group)
                .HasForeignKey(p => p.GroupId)
                    .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
