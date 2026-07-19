using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Core.Constants;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SystemSpace;

[Table(nameof(Tax))]
[Index(nameof(Type), IsUnique = true)]
public class Tax () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(75)]
	public virtual string? Name { get; set; }

	[Column(TypeName = ColumnTypes.PERCENTAGE)]
	public virtual decimal Rate { get; set; }

	public virtual bool System { get; set; }

	[Required, MaxLength(100)]
	public virtual string? Type { get; set; }

	public CompanyTax? Company { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Tax>(options =>
		{

		});
	}
}
