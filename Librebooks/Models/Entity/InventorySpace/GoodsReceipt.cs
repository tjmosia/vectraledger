using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.DocumentSpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsReceipt))]
public class GoodsReceipt
{
    public virtual int Id { get; set; }
    public virtual DateOnly Date { get; set; }
    public virtual DateOnly DateReceived { get; set; }
    public virtual string? Number { get; set; }
    public virtual string? Description { get; set; }
    public virtual string? SourceReference { get; set; }
    public virtual bool Posted { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int StatusId { get; set; }

    public DocumentStatus? Status { get; set; }
    public Company? Company { get; set; }
	public ICollection<GoodsIssueLine>? Items { get; set; }
    public PostingRule? PostingRule { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoodsReceipt>(options =>
        {
            options.HasKey(p => p.Id);
            options.Property(p => p.Id).UseIdentityColumn();
			options.HasIndex(p => new { p.CompanyId, p.Id }).IsClustered();
            options.Property(p => p.Description).HasMaxLength(255);
            options.Property(p => p.Number).IsRequired().HasMaxLength(50);

			options.HasOne(p => p.Status)
				.WithMany()
				.HasForeignKey(p => p.StatusId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);
		});
    }
}
