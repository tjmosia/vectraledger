using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.CustomerSpace;
using Librebooks.Models.Entity.DocumentSpace;
using Librebooks.Models.Entity.IdentitySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesDocument))]
public class SalesDocument () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual DateTime Date { get; set; }
	public virtual DateTime DueDate { get; set; }
	public virtual string? Title { get; set; }
	public virtual string? Number { get; set; }
	public virtual string? CustomerReference { get; set; }
	public virtual string? Message { get; set; }
	public virtual string? FooterComment { get; set; }
	public virtual decimal SubTotalAmount { get; set; }
	public virtual decimal VATAmount { get; set; }
	public virtual decimal TotalAmount { get; set; }
	public virtual int TypeId { get; set; }
	public virtual int StatusId { get; set; }
	public virtual int CustomerId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int CustomerInfoId { get; set; }
	public virtual int CompanyInfoId { get; set; }
	public virtual bool Recorded { get; set; }
	public int? SalesPersonId { get; set; }
	public int CreatedById { get; set; }
	public virtual string? CurrencyCode { get; set; }

	public DocumentStatus? Status { get; set; }
	public CompanySalesRep? SalesPerson { get; set; }
	public SalesDocumentCustomerInfo? CustomerInfo { get; set; }
	public DocumentCompanyInfo? CompanyInfo { get; set; }
	public ICollection<SalesDocumentLine>? Lines { get; set; }
	public Company? Company { get; set; }
	public Customer? Customer { get; set; }
	public User? CreatedBy { get; set; }
	public DocumentType? Type { get; set; }
	public SalesQuote? Quote { get; set; }
	public SalesProforma? ProForma { get; set; }
	public SalesInvoice? Invoice { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesDocument>(options =>
		{
			options.HasKey(p => p.Id);
			options.Property(p => p.Id).UseIdentityColumn();
			options.HasIndex(p => new { p.CompanyId, p.CustomerId, p.Id }).IsClustered();
			options.Property(p => p.SubTotalAmount).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.VATAmount).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.TotalAmount).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.Number).HasMaxLength(50);
			options.Property(p => p.FooterComment).HasMaxLength(400);
			options.Property(p => p.Message).HasMaxLength(400);
			options.Property(p => p.Title).HasMaxLength(75);
			options.Property(p => p.CustomerReference).HasMaxLength(50);

			options.HasMany(p => p.Lines)
				.WithOne(p => p.Document)
				.HasForeignKey(p => p.DocumentId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			options.HasOne(p => p.CreatedBy)
				.WithOne()
				.HasForeignKey<SalesDocument>(p => p.CreatedById)
					.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			options.HasOne(p => p.Status)
				.WithMany()
				.HasForeignKey(p => p.StatusId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Type)
				.WithMany()
				.HasForeignKey(p => p.TypeId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Customer)
				.WithMany()
				.HasForeignKey(p => p.CustomerId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
