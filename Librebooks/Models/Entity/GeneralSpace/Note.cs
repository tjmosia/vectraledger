using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.IdentitySpace;
using VectraBooks.Models.Entity.PurchasesSpace;
using VectraBooks.Models.Entity.SalesSpace;
using VectraBooks.Models.Entity.SupplierSpace;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.GeneralSpace
{
	[Table(nameof(Note))]
	public class Note () : VersionedEntityBase()
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }

		[Required]
		[MaxLength(255)]
		public virtual string? Description { get; set; }

		public virtual bool Actionable { get; set; } = false;
		public virtual bool Completed { get; set; } = false;
		public virtual DateTime DateCreated { get; set; }
		public virtual DateTime? DueDate { get; set; }
		public virtual int? CreatorId { get; set; }

		public User? Creator { get; set; }
		public CustomerNote? CustomerNote { get; set; }
		public SupplierNote? SupplierNote { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
			=> builder.Entity<Note>(options =>
			{
				options.HasOne(p => p.Creator)
					.WithOne()
					.HasForeignKey<Note>(options => options.CreatorId)
						.IsRequired(false)
					.OnDelete(DeleteBehavior.SetNull);

				options.HasOne(p=>p.SupplierNote)
					.WithOne(p => p.Note)
					.HasForeignKey<SupplierNote>(options => options.NoteId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);

				options.HasOne(p => p.CustomerNote)
					.WithOne(p => p.Note)
					.HasForeignKey<CustomerNote>(p => p.NoteId)
						.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
	}
}
