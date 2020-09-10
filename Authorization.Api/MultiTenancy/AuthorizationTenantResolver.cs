using System;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Manager;
using Authorization.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SaasKit.Multitenancy;

namespace Authorization.Api.MultiTenancy
{
    /// <summary>
    /// Authorization tenant resolver.
    /// Implements ITenantResolver from SaasKit in order to Identify the Tenant making a request
    /// Extract Tenant Name from url making the request and check whether it exists
    /// </summary>
    public class AuthorizationTenantResolver : ITenantResolver<Tenant>
    {
        private readonly ITenantManager tenantManager;
        private readonly IConfiguration configuration;

        public AuthorizationTenantResolver(ITenantManager tenantManager, IConfiguration configuration)
        {
            this.tenantManager = tenantManager;
            this.configuration = configuration;
        }


        public async Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            var hostName = context.Request.Host.Value.ToLower();
            var defaultTenant = configuration["DefaultTenant"];

            var tenant = await tenantManager.GetTenantByHostname(hostName) ?? await tenantManager.GetTenantByName(defaultTenant);

            if (tenant != null)
            {
                return new TenantContext<Tenant>(tenant);
            }
            return null;
        }
    }
}
