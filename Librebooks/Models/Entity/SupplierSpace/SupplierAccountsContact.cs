using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SupplierSpace
{
	[Table(nameof(SupplierAccountsContact))]
	public class SupplierAccountsContact
	{
		public virtual int SupplierId { get; set; }
		public virtual int SupplierContactId { get; set; }

		public virtual SupplierContact? SupplierContact { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
			=> builder.Entity<SupplierAccountsContact>(options =>
			{
				options.HasKey(p => new { p.SupplierId, p.SupplierContactId })
					.IsClustered();

				options.HasOne(p => p.SupplierContact)
					.WithOne()
					.HasForeignKey<SupplierAccountsContact>(p => p.SupplierContactId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);
			});

	}
}