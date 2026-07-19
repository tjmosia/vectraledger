using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.SupplierSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.PurchasesSpace;

[Table(nameof(PurchaseLedger))]
public class PurchaseLedger
{
	public virtual int Id { get; set; }
	public virtual DateOnly Date { get; set; }
	public virtual string? Reference { get; set; }
	public virtual string? Description { get; set; }
	public virtual decimal CreditAmount { get; set; }
	public virtual decimal DebitAmount { get; set; }
	public virtual int SourceType { get; set; }
	public virtual int SourceId { get; set; }
	public virtual int SupplierId { get; set; }
	public virtual int CompanyId { get; set; }

	public Supplier? Supplier { get; set; }
	public Company? Company { get; set; }
	public PurchaseLedgerJournal? Journal { get; set; }
	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PurchaseLedger>(entity =>
		{
			// Table
			entity.ToTable(nameof(PurchaseLedger));

			// Key
			entity.HasKey(x => x.Id);
			entity.Property(x => x.Id).UseIdentityColumn();

			// Indexes
			entity.HasIndex(p => new { p.CompanyId, p.SupplierId, p.Id })
				.IsClustered();

			// Properties
			entity.Property(x => x.Reference)
				.IsRequired()
				.HasMaxLength(50);

			entity.Property(x => x.Description)
				.IsRequired()
				.HasMaxLength(155);

			entity.Property(x => x.Date)
				.HasColumnType(ColumnTypes.DATE);

			entity.Property(x => x.CreditAmount)
				.HasColumnType(ColumnTypes.MONETARY);

			entity.Property(x => x.DebitAmount)
				.HasColumnType(ColumnTypes.MONETARY);

			// Relationships
			entity.HasOne(p => p.Supplier)
				.WithMany()
				.HasForeignKey(p => p.SupplierId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}

}
