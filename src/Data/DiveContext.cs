using System.ComponentModel;
using Dive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dive.Data
{
    public class DiveContext : IdentityDbContext<User, Role, int>
    {
        public DiveContext(DbContextOptions<DiveContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity => entity.ToTable(name: "Users"));
            builder.Entity<User>().Ignore(c => c.PhoneNumber).Ignore(c => c.PhoneNumberConfirmed).Ignore(c => c.TwoFactorEnabled);
            builder.Entity<Role>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable(name: "UserClaims"));
            builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable(name: "RoleClaims"));
            builder.Entity<IdentityUserRole<int>>(entity => entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable(name: "UserTokens"));
        }

        public DbSet<Board> Boards { get; set; }
        public override DbSet<User> Users { get; set; }
    }
}