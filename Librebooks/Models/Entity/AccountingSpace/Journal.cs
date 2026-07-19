using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.AccountingSpace;

[Table(nameof(Journal))]
public class Journal () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual DateOnly DateCreated { get; set; }
	public virtual string? Description { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual bool Posted { get; set; }
	public virtual int? SourceType { get; set; }
	public virtual int? SourceId { get; set; }

	public Company? Company { get; set; }

	public ICollection<JournalLine>? Entries { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Journal>(options =>
		{
			options.HasKey(p => p.Id);
			options.Property(p => p.Id).UseIdentityColumn();
			options.Property(p => p.Description).IsRequired().HasMaxLength(255);

			options.HasIndex(p => new { p.CompanyId, p.Id })
				.IsClustered()
				.IsUnique();

			options.HasMany(p => p.Entries)
				.WithOne(p => p.Journal)
				.HasForeignKey(p => p.JournalId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Company).WithOne()
				.HasForeignKey<Journal>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
