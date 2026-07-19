using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.AccountingSpace;

[Table(nameof(LedgerAccountCashFlowType))]
[Index(nameof(LedgerAccountCashFlowType), IsUnique = true)]
public class LedgerAccountCashFlowType () : VersionedEntityBase()
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(75)]
	public virtual string? Name { get; set; }

	[MaxLength(255)]
	public virtual string? Description { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<LedgerAccountCashFlowType>(options =>
		{

		});
	}
}
