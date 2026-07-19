using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Librebooks.Models.Entity.CompanySpace;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Librebooks.Models.Entity.IdentitySpace;

public class User : IdentityUser<int>
{
    [Required]
    [MaxLength(50)]
    public virtual string? Name { get; set; }

    [Required]
    [MaxLength(50)]
    public virtual string? Surname { get; set; }

    [MaxLength(155)]
    public virtual string? Photo { get; set; }

    public virtual DateTime DateRegistered { get; set; }
    public virtual DateTime DateLastLoggedIn { get; set; }

    public ICollection<UserRole>? Roles { get; set; }
    public ICollection<UserLogin>? Logins { get; set; }
    public ICollection<UserToken>? Tokens { get; set; }
    public ICollection<UserClaim>? Claims { get; set; }
    public ICollection<CompanyUser>? Companies { get; set; }

    public static void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(options =>
        {
            options.ToTable(nameof(User));

            options.HasMany(p => p.Roles)
                .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            options.HasMany(p => p.Logins)
                .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            options.HasMany(p => p.Tokens)
                .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            options.HasMany(p => p.Claims)
                .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            options.HasMany(p => p.Companies)
                .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
