using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Librebooks.Core.Constants;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.SupplierSpace;
using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.PurchasesSpace
{
	[Table(nameof(PurchaseLedger))]
	public class PurchaseLedger
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }

		[MaxLength(50), Required]
		public virtual string? Reference { get; set; }

		[MaxLength(155), Required]
		public virtual string? Description { get; set; }

		[Column(TypeName = ColumnTypes.DATE)]
		public virtual DateTime Date { get; set; }


		[Column(TypeName = ColumnTypes.MONETARY)]
		public virtual decimal SubTotal { get; set; }


		[Column(TypeName = ColumnTypes.MONETARY)]
		public virtual decimal TaxAmount { get; set; }


		[Column(TypeName = ColumnTypes.MONETARY)]
		public virtual decimal GrandTotal { get; set; }


		[Required, Column(TypeName = "CHAR(1)")]
		public virtual string? Type { get; set; }


		[Required, Column(TypeName = "CHAR(1)")]
		public virtual int SourceType { get; set; }
		public virtual int SourceId { get; set; }
		public virtual int SupplierId { get; set; }
		public virtual int CompanyId { get; set; }

		public PurchaseDocument? Document { get; set; }
		public Supplier? Supplier { get; set; }
		public Company? Company { get; set; }
		public PurchaseLedgerJournal? Journal { get; set; }
		public static void OnModelCreating (ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PurchaseLedger>(entity =>
			{
				entity.HasIndex(p => new { p.CompanyId, p.SupplierId, p.Id })
					.IsClustered();

				entity.HasOne(p => p.Supplier)
					.WithMany()
					.HasForeignKey(p => p.SupplierId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
			});
		}

	}
}
