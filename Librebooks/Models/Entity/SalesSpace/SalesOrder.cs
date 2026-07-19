using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesOrder))]
public class SalesOrder
{
	[Key]
	public virtual int DocumentId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int CustomerId { get; set; }

	public virtual SalesDocument? Document { get; set; }
	public virtual ICollection<SalesOrderInvoice>? Invoices { get; set; }
	public virtual ICollection<SalesQuoteOrder>? Quotes { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesOrder>(options =>
		   {
			   options.HasIndex(p => new { p.CustomerId, p.CompanyId })
				   .IsClustered();

			   options.HasOne<Company>()
				   .WithMany()
				   .HasForeignKey(p => p.CompanyId)
				   .IsRequired()
				   .OnDelete(DeleteBehavior.Restrict);

			   options.HasOne<Customer>()
				   .WithMany()
				   .HasForeignKey(p => p.CustomerId)
					   .IsRequired(true)
				   .OnDelete(DeleteBehavior.Restrict);

			   options.HasMany(p => p.Invoices)
				   .WithOne(p => p.Order)
				   .HasForeignKey(p => p.OrderId)
					   .IsRequired(true)
				   .OnDelete(DeleteBehavior.Restrict);

			   options.HasOne(p => p.Document)
				   .WithOne()
				   .HasForeignKey<SalesOrder>(p => p.DocumentId)
					   .IsRequired()
				   .OnDelete(DeleteBehavior.Cascade);

			   options.HasMany(p => p.Quotes)
				   .WithOne(p => p.Order)
				   .HasForeignKey(p => p.OrderId)
					   .IsRequired(true)
				   .OnDelete(DeleteBehavior.Restrict);
		   });
	}
}
