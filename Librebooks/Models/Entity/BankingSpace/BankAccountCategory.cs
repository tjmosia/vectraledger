using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.BankingSpace;

[Table(nameof(BankAccountCategory))]
public class BankAccountCategory () : VersionedEntityBase()
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    [Required, MaxLength(50)]
    public virtual string? Name { get; set; }

    [MaxLength(75), Required]
    public virtual string? Type { get; set; }

    [MaxLength(255)]
    public virtual string? Description { get; set; }

    public static void OnModelCreating (ModelBuilder builder)
    {
        builder.Entity<BankAccountCategory>(options =>
        {

        });
    }
}