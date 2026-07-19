using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.IdentitySpace;

public class UserRole : IdentityUserRole<int>
{
	[MaxLength(100)]
	public virtual string? AssociatedTo { get; set; }

	public virtual Role? Role { get; set; }
	public virtual User? User { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<UserRole>(options =>
		{
			options.ToTable(nameof(UserRole));
		});
	}
}
