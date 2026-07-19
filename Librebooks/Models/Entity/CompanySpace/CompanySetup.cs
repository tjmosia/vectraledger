using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CustomerSpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CompanySpace;

[Table(nameof(CompanySetup))]
public class CompanySetup () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[MaxLength(10)]
	public virtual string? Prefix { get; set; }

	[MaxLength(10)]
	public virtual string? Suffix { get; set; }

	[MaxLength(12)]
	public virtual string? NumberFormat { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Customer>(options =>
		   {
			   options.ToTable(nameof(CompanySetup))
				   .HasKey(x => x.Id)
				   .IsClustered();
		   });
	}
}
