using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.IdentitySpace;

public class Role : IdentityRole<int>
{
	public ICollection<RoleClaim>? Claims { get; set; }
	public ICollection<UserRole>? Users { get; set; }

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<Role>(options =>
		{
			options.ToTable(nameof(Role));

			options.HasMany(p => p.Claims)
				.WithOne(p => p.Role)
					.HasForeignKey(p => p.RoleId)
				.OnDelete(DeleteBehavior.Restrict);

			options.HasMany(p => p.Users)
				.WithOne(p => p.Role)
					.HasForeignKey(p => p.RoleId)
				.OnDelete(DeleteBehavior.Restrict);
		});
	}
}
