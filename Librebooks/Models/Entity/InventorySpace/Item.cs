using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Core.Constants;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.AccountingSpace;
using VectraBooks.Models.Entity.CompanySpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(Item))]
public class Item : VersionedEntityBase
{
	public virtual int Id { get; set; }
	public virtual string? Code { get; set; }
	public virtual string? Description { get; set; }
	public virtual string? UOM { get; set; }
	public virtual bool Physical { get; set; }
	public virtual int? CategoryId { get; set; }
	public virtual int TaxId { get; set; }
	public virtual int CompanyId { get; set; }
	public virtual int Active { get; set; }
	public virtual int DebitLedgerAccountId { get; set; }
	public virtual int CreditLedgerAccountId { get; set; }

	public virtual Company? Company { get; set; }
	public virtual ItemCategory? Category { get; set; }
	public IList<Inventory>? Inventories { get; set; }
	public virtual CompanyTax? TaxType { get; set; }
	public virtual ICollection<InventoryAdjustment>? StockAdjustments { get; set; }
	public virtual ICollection<ItemPriceHistory>? PriceHistory { get; set; }
	public virtual ICollection<ItemDetail>? Details { get; set; }
	public LedgerAccount? InventoryLedgerAccount { get; set; }
	public LedgerAccount? CreditLedgerAccount { get; set; }
	public LedgerAccount? DebitLedgerAccount { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Item>(options =>
		{
			// Key
			options.HasKey(x => x.Id);
			options.Property(x => x.Id).UseIdentityColumn();

			// Indexes
			options.HasIndex(p => new { p.CompanyId, p.Code })
				.IsUnique();

			// Properties
			options.Property(x => x.Code)
				.IsRequired()
				.HasMaxLength(50);

			options.Property(x => x.Description)
				.IsRequired()
				.HasMaxLength(255);

			options.Property(x => x.UOM)
				.HasMaxLength(20);

			// Relationships
			options.HasMany(p => p.Inventories)
				.WithOne(p => p.Item)
				.HasForeignKey(p => p.ItemId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			options.HasMany(p => p.PriceHistory)
				.WithOne(p => p.Item)
				.HasForeignKey(p => p.ItemId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);

			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.CreditLedgerAccount)
				.WithOne()
				.HasForeignKey<Item>(p => p.CreditLedgerAccountId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			options.HasOne(p => p.DebitLedgerAccount)
				.WithOne()
				.HasForeignKey<Item>(p => p.DebitLedgerAccountId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
