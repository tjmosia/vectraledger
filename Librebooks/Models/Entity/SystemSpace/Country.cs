using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SystemSpace;

[Table(nameof(Country))]
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Code), IsUnique = true)]
public class Country () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[MaxLength(75), Required]
	public virtual string? Name { get; set; }

	[MaxLength(3), Required]
	public virtual string? Code { get; set; }

	[MaxLength(15)]
	public virtual string? DialingCode { get; set; }

	public void NormalizeCode ()
		=> Code = Code?.ToUpper();

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Country>(options =>
		{

		});
	}
}
