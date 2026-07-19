using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesCredit))]
public class SalesCredit
{
    [Key]
    public virtual int DocumentId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int CustomerId { get; set; }

	public virtual Company? Company { get; set; }
	public virtual Customer? Customer{ get; set; }
    public virtual SalesDocument? Document { get; set; }
    public virtual ICollection<SalesInvoiceCredit>? CreditedInvoices { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesCredit>(options =>
			{
				options.HasIndex(p => new { p.CompanyId, p.DocumentId })
					.IsClustered();

				options.HasOne(p => p.Document)
					.WithOne()
					.HasForeignKey<SalesCredit>(p => p.DocumentId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);

				options.HasMany(p => p.CreditedInvoices)
					.WithOne(p => p.Credit)
					.HasForeignKey(p => p.CreditId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				options.HasOne(p => p.Company)
					.WithMany()
					.HasForeignKey(p => p.CompanyId)
					.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				options.HasOne(p => p.Customer)
					.WithMany()
					.HasForeignKey(p => p.CustomerId)
					.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);
			});
	}
}
