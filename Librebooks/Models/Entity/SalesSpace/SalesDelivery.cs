using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Models.Entity.CompanySpace;

namespace VectraBooks.Models.Entity.SalesSpace;

[Table(nameof(SalesDelivery))]
public class SalesDelivery
{
	public int DocumentId { get; set; }
	public virtual DateOnly DateDelivered { get; set;  }
	public virtual bool Printed { get; set; }
	public virtual bool Posted { get; set; }
	public virtual int CompanyId { get; set; }

	public Company? Company { get; set; }
	public SalesDocument? Document { get; set; }

	public	static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<SalesDelivery>(options =>
		{
			options.HasKey(p => p.DocumentId);
			options.Property(p => p.DocumentId).ValueGeneratedNever();
			options.HasIndex( p=> new {p.CompanyId, p.DocumentId }).IsUnique().IsClustered();
		});
	}
}
