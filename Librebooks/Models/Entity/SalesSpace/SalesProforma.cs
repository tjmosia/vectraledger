using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.SalesSpace;

[Table(nameof(SalesProforma))]
public class SalesProforma
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
	public virtual int Id { get; set; }

	public static void OnModelCreating (ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<SalesProforma>(options =>
		{

		});
	}
}
