using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;
using Librebooks.Models.Entity.GeneralSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesPerson))]
public class SalesPerson
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public virtual int ContactId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int? CompanyUserId { get; set; }

	public virtual Contact? Contact { get; set; }
	public virtual CompanyUser? CompanyUser { get; set; }

	public SalesPerson () { }

	public SalesPerson (int companyId, int contactId, int? companyUserId = null)
		: this()
	{
		CompanyId = companyId;
		CompanyUserId = companyUserId;
		ContactId = contactId;
	}

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesPerson>(options =>
		   {
			   options.ToTable(nameof(SalesPerson))
				   .HasKey(p => p.ContactId)
				   .IsClustered(false);

			   options.HasIndex(p => new { p.CompanyId, p.ContactId })
				   .IsUnique()
				   .IsClustered();

			   options.HasOne<Company>()
				   .WithMany()
				   .HasForeignKey(p => p.CompanyId)
				   .IsRequired()
				   .OnDelete(DeleteBehavior.Restrict);

			   options.HasMany<Customer>()
				   .WithOne(p => p.SalesPerson)
				   .HasForeignKey(p => p.SalesPersonId)
					   .IsRequired(false)
				   .OnDelete(DeleteBehavior.Restrict);
		   });
	}
}