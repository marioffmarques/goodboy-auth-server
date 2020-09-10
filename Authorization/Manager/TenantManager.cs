using System;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Repository;
using Microsoft.Extensions.Configuration;

namespace Authorization.Manager
{
    public class TenantManager : ITenantManager
    {
        private readonly ITenantRepository tenantRepository;
        private readonly IConfiguration configuration;

        public TenantManager(ITenantRepository tenantRepository, IConfiguration configuration)
        {
            this.tenantRepository = tenantRepository;
            this.configuration = configuration;
        }

        public Task<Tenant> GetTenantById(Guid tenantId, CancellationToken cancelationToken = default(CancellationToken))
        {
            return tenantRepository.GetAsync(tenantId, cancelationToken);
        }

        public Task<Tenant> GetTenantByHostname(string hostname, CancellationToken cancelationToken = default(CancellationToken))
        {
            return tenantRepository.GetTenantByHostname(hostname, cancelationToken);
        }

        public Task<Tenant> GetTenantByName(string tenantName, CancellationToken cancelationToken = default(CancellationToken))
        {
            return tenantRepository.GetTenantByName(tenantName, cancelationToken);
        }

        public async Task<(Tenant, ExceptionKey?)> Add(Tenant tenant, CancellationToken cancelationToken = default(CancellationToken))
        {
            tenant.Name = tenant.DisplayName.Slugify();

            if (await tenantRepository.GetTenantByName(tenant.Name, cancelationToken) != null)
            {
                return (null, ExceptionKey.ERROR_IN_USE);
            }

            tenant.Host = String.Format("{0}.{1}", tenant.Name, configuration["ClientDomain"]);


            tenantRepository.Add(tenant, cancelationToken);

            if (await tenantRepository.Commit(cancelationToken) > 0)
            {
                return (tenant, null);
            }
            else
            {
                return (null, ExceptionKey.ERROR_CREATE);
            }
        }
    }
}
