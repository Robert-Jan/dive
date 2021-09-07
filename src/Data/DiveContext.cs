using System.ComponentModel;
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

            builder.Entity<User>(entity => entity.ToTable(name: "Users"));
            builder.Entity<User>().Ignore(c => c.PhoneNumber).Ignore(c => c.PhoneNumberConfirmed).Ignore(c => c.TwoFactorEnabled);
            builder.Entity<Role>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable(name: "UserClaims"));
            builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable(name: "RoleClaims"));
            builder.Entity<IdentityUserRole<int>>(entity => entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable(name: "UserTokens"));

            builder.Entity<Post>().HasOne(a => a.AcceptedAnswer).WithOne()
                .HasForeignKey<Post>(a => a.AcceptedAnswerId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Post>().HasOne(a => a.Parent).WithOne()
                .HasForeignKey<Post>(a => a.ParentId).OnDelete(DeleteBehavior.Restrict);
        }

        public override DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}