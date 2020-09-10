using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Repository.EntityConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).HasDefaultValueSql("uuid_in((md5((random())::text))::cstring)");

            builder.HasOne(o => o.Tenant)
                   .WithMany(t => t.Users);
            
            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(256);
            builder.HasAlternateKey(prop => new { prop.UserName, prop.TenantId });
        }
    }
}
