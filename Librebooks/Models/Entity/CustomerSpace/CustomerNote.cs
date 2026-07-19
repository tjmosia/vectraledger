using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Models.Entity.GeneralSpace;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CustomerSpace;

[Table(nameof(CustomerNote))]
public class CustomerNote
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public virtual int NoteId { get; set; }

    public virtual int CustomerId { get; set; }

    public virtual Note? Note { get; set; }
    public virtual Customer? Customer { get; set;  }

    public static void OnModelCreating (ModelBuilder builder)
    {
        builder.Entity<CustomerNote>(options =>
        {
            options.HasIndex(p => new { p.CustomerId, p.NoteId })
                .IsClustered();
        });
    }
}
