using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.DocumentSpace;

[Table(nameof(DocumentSetup))]
public class DocumentSetup () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[MaxLength(75)]
	public virtual int TypeId { get; set; }

	[MaxLength(50), Required]
	public virtual string? Title { get; set; }

	[MaxLength(20)]
	public virtual string? Prefix { get; set; }

	[MaxLength(20)]
	public virtual string? Suffix { get; set; }

	public virtual int NextNumber { get; set; }
	public virtual bool System { get; set; }

	[MaxLength(500)]
	public virtual string? FooterMessage { get; set; }

	[MaxLength(500)]
	public virtual string? NoteMessage { get; set; }

	public virtual int? CompanyId { get; set; }

	public virtual int PrintTemplateId { get; set; }

	public DocumentPrintTemplate? PrintTemplate { get; set; }
	public DocumentType? Type { get; set; }
	public Company? Company { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
		=> builder.Entity<DocumentSetup>(options =>
		{
			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsUnique()
				.IsClustered();

			options.HasOne(p => p.Company)
				.WithMany(p => p.DocumentSetups)
				.HasForeignKey(p => p.CompanyId)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.Cascade);

			options.HasOne(p => p.Type).WithMany()
				.HasForeignKey(p => p.TypeId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
}
