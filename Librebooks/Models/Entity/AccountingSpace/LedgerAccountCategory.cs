using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.AccountingSpace;

[Table(nameof(LedgerAccountCategory))]
[Index(nameof(Name), IsUnique = true)]
public class LedgerAccountCategory () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(75)]
	public virtual string? Name { get; set; }

	[Required, MaxLength(2)]
	public virtual string? ShortName { get; set;  }

	[MaxLength(255)]
	public virtual string? Description { get; set; }

	[MaxLength(75)]
	public virtual string? ClassType { get; set; }

	public virtual int CashFlowTypeId { get; set; }

	public LedgerAccountCashFlowType? CashFlowType { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<LedgerAccountCategory>(options =>
		{
			options.HasOne(p => p.CashFlowType)
				.WithMany()
				.HasForeignKey(p => p.CashFlowTypeId)
					.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}