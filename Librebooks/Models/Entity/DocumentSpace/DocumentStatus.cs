using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.PurchasesSpace;
using VectraBooks.Models.Entity.SalesSpace;

namespace VectraBooks.Models.Entity.DocumentSpace;

[Table(nameof(DocumentStatus))]
public class DocumentStatus () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual string? Name { get; set; }
	public virtual string? Color { get; set; }
	public virtual int DocumentTypeId { get; set; }

	public DocumentType? DocumentType { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<DocumentStatus>(options =>
		{
			options.HasKey(p=>p.Id).IsClustered();
			options.Property(p => p.Id).UseIdentityColumn();
			options.HasIndex(p => p.Name).IsUnique();
			options.Property(p => p.Color).HasMaxLength(155);
			options.Property(p => p.Name).IsRequired().HasMaxLength(75);

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
