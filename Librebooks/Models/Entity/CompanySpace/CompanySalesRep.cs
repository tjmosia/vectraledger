using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.CustomerSpace;
using Librebooks.Models.Entity.GeneralSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.CompanySpace;

[Table(nameof(CompanySalesRep))]
public class CompanySalesRep()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public virtual int ContactId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int? CompanyUserId { get; set; }

	public Contact? Contact { get; set; }
	public CompanyUser? CompanyUser { get; set; }
	public ICollection<Customer>? Customers { get; set; }

	public CompanySalesRep (int companyId, int contactId, int? companyUserId = null)
		: this()
	{
		CompanyId = companyId;
		CompanyUserId = companyUserId;
		ContactId = contactId;
	}

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CompanySalesRep>(options =>
		   {
			   options.ToTable(nameof(CompanySalesRep))
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

			   options.HasMany(p=>p.Customers)
				   .WithOne(p => p.SalesRep)
				   .HasForeignKey(p => p.SalesRepId)
					   .IsRequired(false)
				   .OnDelete(DeleteBehavior.Restrict);
		   });
	}
}