using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Repository.EntityConfig
{
    public class TenantConfig : IEntityTypeConfiguration<Tenant>
    {
        
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.Property(u => u.Id).HasDefaultValueSql("uuid_in((md5((random())::text))::cstring)");
            builder.Property(u => u.Name).IsRequired();
            builder.HasIndex(u => u.Name).HasName("TenantNameIndex").IsUnique();

            builder.HasMany(o => o.Users)
                   .WithOne(t => t.Tenant);
        }
    }
}
