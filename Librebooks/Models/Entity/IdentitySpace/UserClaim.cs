using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.IdentitySpace
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User? User { get; set; }

        public static void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<UserClaim>(options =>
            {
                options.ToTable(nameof(UserClaim));
            });
        }
    }
}
