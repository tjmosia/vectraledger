using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.GeneralSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.CompanySpace;

[Table(nameof(CompanyBuyer))]
public class CompanyBuyer
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int ContactId { get; set; }
	public virtual int CompanyUserId { get; set; }

	public virtual Contact? Contact { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CompanyBuyer>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsClustered();

			options.HasOne<CompanyUser>()
				.WithOne()
				.HasForeignKey<CompanyBuyer>(p => p.CompanyUserId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne<Company>()
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}