using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.AccountingSpace;

[Table(nameof(LedgerAccount))]
public class LedgerAccount () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual string? Name { get; set; }
	public virtual int TaxId { get; set; }
	public virtual string? Description { get; set; }
	public virtual int? ParentId { get; set; }
	public virtual bool Active { get; set; }
	public virtual bool System { get; set; }
	public virtual int CategoryId { get; set; }

	public LedgerAccount? Parent { get; set; }
	public LedgerAccountCategory? Category { get; set; }
	public CompanyTax? Tax { get; set; }

	public ICollection<LedgerAccount>? ChildAccounts { get; set; }
	public ICollection<CompanyLedgerAccount>? CompanyAccounts { get; set; }

	public ICollection<JournalLine>? JournalEntries { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
		=> builder.Entity<LedgerAccount>(options =>
		{
			// Table
			options.ToTable(nameof(LedgerAccount));

			// Key
			options.HasKey(x => x.Id);
			options.Property(x => x.Id).UseIdentityColumn();

			// Properties
			options.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(75);

			options.Property(x => x.Description)
				.IsRequired()
				.HasMaxLength(255);

			// Relationships
			options.HasMany(p => p.ChildAccounts)
				.WithOne(p => p.Parent)
				.HasForeignKey(p => p.ParentId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Category)
				.WithMany()
				.HasForeignKey(p => p.CategoryId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Tax)
			   .WithMany()
			   .HasForeignKey(p => p.TaxId)
				   .IsRequired()
			   .OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.CompanyAccounts)
				.WithOne(p => p.Account)
				.HasForeignKey(p => p.AccountId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
}
