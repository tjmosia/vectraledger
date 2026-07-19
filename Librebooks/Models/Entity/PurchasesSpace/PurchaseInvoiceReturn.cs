using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.PurchasesSpace
{
	[Table(nameof(PurchaseInvoiceReturn))]
	public class PurchaseInvoiceReturn
	{
		public virtual int ReturnId { get; set; }
		public virtual int InvoiceId { get; set; }

		[MaxLength(255)]
		public virtual string? Comment { get; set; }

		public virtual PurchaseDocument? Return { get; set; }
		public virtual PurchaseDocument? Invoice { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
		{
			builder.Entity<PurchaseInvoiceReturn>(options =>
			{
				options.HasKey(x => new { x.InvoiceId, x.ReturnId })
					.IsClustered();
			});
		}
	}
}
