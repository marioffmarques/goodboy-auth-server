using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Repository;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Api
{
    public class DBInitializer
    {
        private RoleManager<Role> _roleMgr;
        private IConfiguration _config;
       
        public DBInitializer(RoleManager<Role> roleMgr, IConfiguration config)
        {
            _roleMgr = roleMgr;
            _config = config;
        }

        public async Task Seed(IApplicationBuilder app)
        {
            if (!(await _roleMgr.RoleExistsAsync("Admin")))
            {
                var role = new Role();
                role.Name = "Admin";
                await _roleMgr.CreateAsync(role);
            }
            if (!(await _roleMgr.RoleExistsAsync("User")))
            {
                var role = new Role();
                role.Name = "User";
                await _roleMgr.CreateAsync(role);
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var authcontext = serviceScope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
                var tenantId = Guid.Empty;
                if (!authcontext.Tenants.Any())
                {
                    var tenant = new Tenant
                    {
                        DisplayName = _config.GetValue<string>("DefaultTenant"),
                        IsActive = true,
                        Name = _config["DefaultTenant"].ToLower(),
                        Host = String.Format("{0}.{1}", _config["DefaultTenant"].ToLower(), _config["ClientDomain"])
                    };
                    authcontext.Tenants.Add(tenant);
                    authcontext.SaveChanges();
                    tenantId = tenant.Id;
                }


                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerEntities.GetClients())
                    {
                        client.Properties = new Dictionary<string, string> { { "TenantId", tenantId.ToString() } };
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerEntities.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerEntities.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }


    public static class IdentityServerEntities
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("authorizationresourcesapi", "Identity Server Resources management api")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "authorizationresourcesapi" },
                    AccessTokenLifetime = 60 * 60 * 24 * 7 // 7 days
                    //Properties = new Dictionary<string, string> { { "TenantId", "ebb56964-be95-46b9-ad22-f02322c52ab8" } }
                }
            };
        }
    }


}
