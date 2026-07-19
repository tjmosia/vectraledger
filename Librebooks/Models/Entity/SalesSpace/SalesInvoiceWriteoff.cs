using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Core.Constants;
using Librebooks.Models.Entity;
using Librebooks.Models.Entity.CustomerSpace;

using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.SalesSpace;

[Table(nameof(SalesInvoiceWriteoff))]
public class SalesInvoiceWriteoff
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }
	public virtual int InvoiceId { get; set; }
	public virtual int WriteOffId { get; set; }

	[Column(TypeName = ColumnTypes.MONETARY)]
	public virtual decimal Amount { get; set; }

	public virtual CustomerWriteOff? WriteOff { get; set; }
	public virtual SalesInvoice? Invoice { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesInvoiceWriteoff>(options =>
		{
			options.HasIndex(p => new { p.InvoiceId, p.WriteOffId })
				.IsClustered();
		});
	}
}