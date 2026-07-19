using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.IdentitySpace
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User? User { get; set; }

        public static void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<UserLogin>(options =>
            {
                options.ToTable(nameof(UserLogin));
            });
        }
    }
}
