using Dive.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dive.App.Data
{
    public class DiveContext : IdentityDbContext<User, Role, int>
    {
        public DiveContext(DbContextOptions<DiveContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity => entity.ToTable(name: "users"));
            builder.Entity<User>().Ignore(c => c.PhoneNumber).Ignore(c => c.PhoneNumberConfirmed).Ignore(c => c.TwoFactorEnabled);
            builder.Entity<Role>(entity => entity.ToTable(name: "roles"));
            builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable(name: "user_claims"));
            builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable(name: "user_logins"));
            builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable(name: "role_claims"));
            builder.Entity<IdentityUserRole<int>>(entity => entity.ToTable(name: "user_roles"));
            builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable(name: "user_tokens"));
        }

        public override DbSet<User> Users { get; set; }
    }
}