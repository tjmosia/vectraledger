using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.BankingSpace;
using Librebooks.Models.Entity.PurchasesSpace;
using Librebooks.Models.Entity.SalesSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SystemSpace
{
	[Table(nameof(PaymentMethod))]
	[Index(nameof(Name), IsUnique = true)]
	public class PaymentMethod () : VersionedEntityBase()
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }

		[Required, MaxLength(50)]
		public virtual string? Name { get; set; }

		[MaxLength(255)]
		public virtual string? Description { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
			=> builder.Entity<PaymentMethod>(options =>
			{
				options.HasMany<BankAccount>()
					.WithOne(p => p.PaymentMethod)
					.HasForeignKey(p => p.PaymentMethodId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				options.HasMany<SalesReceipt>()
					.WithOne(p => p.PaymentMethod)
					.HasForeignKey(p => p.PaymentMethodId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				options.HasMany<PurchasePayment>()
					.WithOne(p => p.PaymentMethod)
					.HasForeignKey(p => p.PaymentMethodId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);
			});
	}
}
