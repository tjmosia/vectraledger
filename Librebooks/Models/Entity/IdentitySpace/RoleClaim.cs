using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.IdentitySpace
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role? Role { get; set; }

        public static void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<RoleClaim>(options =>
            {
                options.ToTable(nameof(RoleClaim));
            });
        }
    }
}
