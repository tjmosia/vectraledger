using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Core.Constants;
using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.BankingSpace;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.SupplierSpace;
using Librebooks.Models.Entity.SystemSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.PurchasesSpace
{
	[Table(nameof(PurchasePayment))]
	public class PurchasePayment () : VersionedEntityBase()
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }
		public virtual DateTime Date { get; set; }

		[MaxLength(50), Required]
		public virtual string? Number { get; set; }

		[MaxLength(50)]
		public virtual string? Reference { get; set; }

		[MaxLength(255)]
		public virtual string? Description { get; set; }

		[MaxLength(255)]
		public virtual string? Comments { get; set; }

		[Column(TypeName = ColumnTypes.MONETARY)]
		public virtual decimal SubTotal { get; set; }

		[Column(TypeName = ColumnTypes.MONETARY)]
		public virtual decimal TaxAmount { get; set; }

		[Column(TypeName = ColumnTypes.MONETARY)]
		public virtual decimal GrandAmount { get; set; }

		public virtual bool Reconciled { get; set; }
		public virtual bool Recorded { get; set; }
		public virtual int SupplierId { get; set; }
		public virtual int CompanyId { get; set; }
		public virtual int BankAccountId { get; set; }
		public virtual int PaymentMethodId { get; set; }

		public virtual PaymentMethod? PaymentMethod { get; set; }
		public virtual BankAccount? BankAccount { get; set; }
		public virtual ICollection<PurchaseInvoicePayment>? AllocatedInvoices { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
		{
			builder.Entity<PurchasePayment>(options =>
			{
				options.HasOne(p => p.PaymentMethod)
					.WithMany()
					.HasForeignKey(p => p.PaymentMethodId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				options.HasMany(p => p.AllocatedInvoices)
					.WithOne(p => p.Payment)
					.HasForeignKey(p => p.PaymentId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				options.HasOne<Supplier>()
					.WithMany()
					.HasForeignKey(p => p.SupplierId)
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
}
