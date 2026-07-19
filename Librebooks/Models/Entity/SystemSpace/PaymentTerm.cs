using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SystemSpace
{
	[Table(nameof(PaymentTerm))]
	[Index(nameof(Name), IsUnique = true)]
	[Index(nameof(ShortName), IsUnique = true)]
	public class PaymentTerm () : VersionedEntityBase()
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }

		[Required, MaxLength(100)]
		public virtual string? Name { get; set; }

		[Required, MaxLength(10)]
		public virtual string? ShortName { get; set; }

		[MaxLength(255)]
		public virtual string? Description { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
		{
			builder.Entity<PaymentTerm>(options =>
			{

			});
		}
	}
}
