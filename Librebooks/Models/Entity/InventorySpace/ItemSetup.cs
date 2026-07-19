using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using VectraBooks.Models.Entity.CompanySpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(ItemSetup))]
public class ItemSetup () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public virtual int CompanyId { get; set; }

	[MaxLength(10)]
	public virtual string? Prefix { get; set; }

	[MaxLength(10)]
	public virtual string? Suffix { get; set; }

	public virtual int NextNumber { get; set; }

    [MaxLength(10)]
    public virtual string? NumberFormat { get; set; }

    public Company? Company { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<ItemSetup>(options =>
		{
			options.HasOne(p => p.Company)
				.WithOne(p => p.ItemSetup)
				.HasForeignKey<ItemSetup>(p => p.CompanyId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
