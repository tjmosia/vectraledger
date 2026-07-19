using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Extensions.Models;
using Librebooks.Models.Entity.CompanySpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.InventorySpace;

[Table(nameof(ItemPriceHistory))]
public class ItemPriceHistory () : VersionedEntityBase()
{
	public virtual int Id { get; set; }
	public virtual DateOnly Date { get; set; }
	public virtual decimal OldPrice { get; set; }
    public virtual decimal NewPrice { get; set; }
	public virtual int ItemId { get; set; }
	public virtual int CompanyId { get; set; }

	public virtual Item? Item { get; set; }
	public virtual Company? Company { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ItemPriceHistory>(options =>
		{
			options.HasKey(e => e.Id);
			options.Property(p => p.Id).UseIdentityColumn();
			options.HasIndex(p => new { p.CompanyId, p.ItemId, p.Id })
				.IsClustered();
			options.Property(p=>p.OldPrice).HasColumnType(ColumnTypes.MONETARY);
			options.Property(p => p.NewPrice).HasColumnType(ColumnTypes.MONETARY);
			options.HasOne(p => p.Company)
				.WithMany()
				.HasForeignKey(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
