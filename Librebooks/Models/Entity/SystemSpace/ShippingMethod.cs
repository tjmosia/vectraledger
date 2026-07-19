using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.SalesSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SystemSpace
{
	[Table(nameof(ShippingMethod))]
	[Index(nameof(Name), IsUnique = true)]
	[Index(nameof(ShortName), IsUnique = true)]
	public class ShippingMethod () : VersionedEntityBase()
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }

		[Required, MaxLength(100)]
		public virtual string? Name { get; set; }

		[MaxLength(20)]
		public virtual string? ShortName { get; set; }

		[MaxLength(255)]
		public virtual string? Description { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
		{
			builder.Entity<ShippingMethod>(options =>
			{
				options.HasMany<SalesDocument>()
					.WithOne(p => p.ShippingMethod)
					.HasForeignKey(p => p.ShippingMethodId)
						.IsRequired(false)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
