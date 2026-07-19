using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.SalesSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SystemSpace;

[Table(nameof(ShippingTerm))]
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(ShortName), IsUnique = true)]
public class ShippingTerm () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required]
	[MaxLength(155)]
	public virtual string? Name { get; set; }

	[MaxLength(10)]
	public virtual string? ShortName { get; set; }

	[MaxLength(255)]
	public virtual string? Description { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<ShippingTerm>(options =>
		{
			options.HasMany<SalesDocument>()
				.WithOne(p => p.ShippingTerm)
				.HasForeignKey(p => p.ShippingTermId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

		});
	}
}
