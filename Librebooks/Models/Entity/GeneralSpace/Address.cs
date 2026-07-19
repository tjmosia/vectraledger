using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.GeneralSpace;

[Table(nameof(Address))]
public class Address(): VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual bool IsPhysical { get; set; }
	public virtual string? Street { get; set; }
	public virtual string? Suburb { get; set; }
	public virtual string? CityOrTown { get; set; }
	public virtual string? Province { get; set; }
	public virtual string? PostalCode { get; set; }
	public virtual string? CountryCode { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual decimal? GPSLongitude { get; set;  }
	public virtual decimal? GPSLatitude { get; set;  }


	public Company? Company { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Address>(options =>
		{
			options.HasKey(p => p.Id);
			options.Property(p => p.Id).UseIdentityColumn();
			options.Property(p => p.Street).HasMaxLength(155).IsRequired();
			options.Property(p => p.Suburb).HasMaxLength(100);
			options.Property(p => p.CityOrTown).HasMaxLength(100).IsRequired();
			options.Property(p => p.Province).HasMaxLength(100).IsRequired();
			options.Property(p => p.PostalCode).HasMaxLength(20);
			options.Property(p => p.CountryCode).IsRequired().HasMaxLength(2);
			options.Property(p => p.GPSLongitude).HasColumnType("decimal(10,6)");
			options.Property(p => p.GPSLatitude).HasColumnType("decimal(9,6)");

			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsClustered()
				.IsUnique();

			options.HasOne<Company>()
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}

}
