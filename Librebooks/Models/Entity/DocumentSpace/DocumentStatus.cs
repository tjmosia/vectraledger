using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.PurchasesSpace;
using Librebooks.Models.Entity.SalesSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.DocumentSpace;

[Table(nameof(DocumentStatus))]
[Index(nameof(Name), IsUnique = true)]
public class DocumentStatus () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(50)]
	public virtual string? Name { get; set; }

	[MaxLength(6)]
	public virtual string? Color { get; set; }

	public virtual int DocumentTypeId { get; set; }

	public DocumentType? DocumentType { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<DocumentStatus>(options =>
		{
			options.HasMany<PurchaseDocument>()
				.WithOne(p => p.Status)
				.HasForeignKey(p => p.StatusId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany<SalesDocument>()
				.WithOne(p => p.Status)
				.HasForeignKey(p => p.StatusId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.DocumentType)
				.WithMany()
				.HasForeignKey(p => p.DocumentTypeId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
