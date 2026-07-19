using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SystemSpace;

[Table(nameof(DateFormat))]
[Index(nameof(Format), IsUnique = true)]
public class DateFormat () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(50)]
	public virtual string? Format { get; set; }
	public virtual bool Default { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<DateFormat>(options =>
		{
		});
	}
}
