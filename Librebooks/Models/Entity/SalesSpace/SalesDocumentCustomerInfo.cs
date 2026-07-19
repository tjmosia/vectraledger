using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.CustomerSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesDocumentCustomerInfo))]
public class SalesDocumentCustomerInfo
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int CustomerId { get; set; }
	public virtual string? CustomerName { get; set; }
	public virtual string? PostalAddress { get; set; }
	public virtual string? ShippingAddress { get; set; }
	public virtual string? VATNumber { get; set; }
	public virtual DateTime DateCreated { get; set; }
	public virtual bool Active { get; set; }

	public Customer? Customer { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesDocumentCustomerInfo>(options =>
		{
			options.Property(p => p.VATNumber)
				 .HasMaxLength(10);

			options.Property(p => p.CustomerName)
				.IsRequired()
				.HasMaxLength(100);

			options.HasIndex(p => p.CustomerId)
				.IsClustered();

			options.HasOne<Customer>()
				.WithOne()
				.HasForeignKey<SalesDocumentCustomerInfo>(p => p.CustomerId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasMany<SalesDocument>()
				.WithOne(p => p.CustomerInfo)
				.HasForeignKey(propa => propa.CustomerInfoId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Customer)
				.WithOne()
				.HasForeignKey<SalesDocumentCustomerInfo>(p => p.CustomerId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);
		});
	}
}
