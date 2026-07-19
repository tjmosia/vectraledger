using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesLedger))]
public class SalesLedger
{
	public virtual int Id { get; set; }
	public virtual string? Reference { get; set; }
	public virtual string? Description { get; set; }
	public virtual DateTime Date { get; set; }
	public virtual decimal DebitAmount { get; set; }
	public virtual decimal CreditAmount { get; set; }
	public virtual int SourceTypeId { get; set; }
	public virtual int SourceId { get; set; }
	public virtual int CustomerId { get; set; }
	public virtual int CompanyId { get; set; }

	public SalesDocument? Document { get; set; }
	public Customer? Customer { get; set; }
	public Company? Company { get; set; }
	public SalesLedgerSourceType? SourceType { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<SalesLedger>(entity =>
		{
			entity.HasKey(p => p.Id);
			entity.Property(p => p.Id).UseIdentityColumn();

			entity.HasIndex(p => new { p.CompanyId, p.CustomerId, p.Id }).IsClustered();

			entity.Property(p => p.Reference).IsRequired().HasMaxLength(50);
			entity.Property(p => p.Description).IsRequired().HasMaxLength(50);

			entity.Property(p => p.CreditAmount)
				.HasColumnType(ColumnTypes.MONETARY);

			entity.Property(p => p.DebitAmount)
				.HasColumnType(ColumnTypes.MONETARY);

			entity.HasOne(p => p.Customer)
				.WithMany()
				.HasForeignKey(p => p.CustomerId)
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
