using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Librebooks.Models.Entity.DocumentSpace;

[Table(nameof(DocumentType))]
[Index(nameof(Name), IsUnique = true)]
public class DocumentType () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public virtual int Id { get; set; }

	[Required, MaxLength(75)]
	public virtual string? Name { get; set; }

	[Required, MaxLength(6)]
	public virtual string? Prefix { get; set; }

	public ICollection<DocumentStatus>? Statuses { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DocumentType>(options =>
		{
			
			options.HasMany(p => p.Statuses)
				.WithOne(p => p.DocumentType)
				.HasForeignKey(p => p.DocumentTypeId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
