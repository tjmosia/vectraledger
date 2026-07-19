using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.DocumentSpace
{
	[Table(nameof(DocumentPrintTemplate))]
	public class DocumentPrintTemplate () : VersionedEntityBase()
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int Id { get; set; }

		[Required, MaxLength(75)]
		public virtual string? Name { get; set; }

		[MaxLength(255)]
		public virtual string? Description { get; set; }

		[MaxLength(155)]
		public virtual string? DocumentType { get; set; }

		public static void OnModelCreating (ModelBuilder builder)
		{
			builder.Entity<DocumentPrintTemplate>(options =>
			   {
				   options.HasMany<DocumentSetup>()
					   .WithOne(p => p.PrintTemplate)
					   .HasForeignKey(p => p.PrintTemplateId)
						   .IsRequired()
					   .OnDelete(DeleteBehavior.Restrict);
			   });
		}
	}
}
