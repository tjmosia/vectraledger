using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VectraBooks.Models.Entity.GeneralSpace;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SupplierSpace
{
	[Table(nameof(SupplierContact))]
	public class SupplierContact
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public virtual int ContactId { get; set; }
		public virtual int SupplierId { get; set; }

		public virtual SupplierAccountsContact? AccountsContact { get; set; }
		public virtual Contact? Contact { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
		{
			builder.Entity<SupplierContact>(options =>
			{
				options.HasIndex(p => p.SupplierId)
					.IsClustered();

				options.HasOne(p => p.AccountsContact)
					.WithOne(p => p.SupplierContact)
					.HasForeignKey<SupplierContact>(p => p.ContactId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}