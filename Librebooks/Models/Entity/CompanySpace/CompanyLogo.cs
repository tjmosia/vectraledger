using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.CompanySpace;

[Table(nameof(CompanyLogo))]
public class CompanyLogo ()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public virtual int CompanyId { get; set; }
	public virtual int ImageId { get; set; }

	public virtual CompanyImage? Image { get; set; }

	public CompanyLogo (int companyId, int imageId)
		: this()
	{
		CompanyId = companyId;
		ImageId = imageId;
	}

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<CompanyLogo>(options =>
		{
		});
	}
}
