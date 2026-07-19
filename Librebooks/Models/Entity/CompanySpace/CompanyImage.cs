using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.DocumentSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.CompanySpace;

[Table(nameof(CompanyImage))]
public class CompanyImage () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual DateOnly DateCreated { get; set; }

	[Required]
	public virtual string? PathName { get; set; }

	public void SetDateCreated (DateTime date)
		=> DateCreated = DateOnly.FromDateTime(date);

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CompanyImage>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsClustered();

			options.HasOne<CompanyLogo>()
				.WithOne(p => p.Image)
				.HasForeignKey<CompanyLogo>(p => p.ImageId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne<Company>()
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany<DocumentCompanyInfo>()
				.WithOne(p => p.Logo)
				.HasForeignKey(p => p.LogoId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
