using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CustomerSpace;

[Table(nameof(CustomerSetup))]
public class CustomerSetup () : VersionedEntityBase()
{
	[Key]
	public virtual int CompanyId { get; set; }

	[MaxLength(20)]
	public virtual string? Prefix { get; set; }

	[MaxLength(20)]
	public virtual string? Suffix { get; set; }

	public virtual int NextNumber { get; set; }

    [MaxLength(10)]
    public virtual string? NumberFormat { get; set; }

    public Company? Company { get; set;  }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CustomerSetup>(options =>
		{
			options.HasOne(p=>p.Company)
				.WithOne(p => p.CustomerSetup)
				.HasForeignKey<CustomerSetup>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
