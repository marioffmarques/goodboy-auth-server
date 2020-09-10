using System;
using Authorization.Repository.EntityConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authorization.Repository
{
    public class AuthorizationDbContext : IdentityDbContext<User, Role, Guid>
    {

        public virtual DbSet<Tenant> Tenants { get; set; }


        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        {           
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new TenantConfig());
        }

    }
}
