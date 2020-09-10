using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Repository
{
    public class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        public TenantRepository(AuthorizationDbContext context) : base(context)
        {
        }

        public Task<Tenant> GetTenantByName(string name, CancellationToken cancelationToken = default(CancellationToken))
        {
            return context.Tenants.FirstOrDefaultAsync(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase), cancelationToken);
        }

        public Task<Tenant> GetTenantByHostname(string hostName, CancellationToken cancelationToken = default(CancellationToken))
        {
            return context.Tenants.FirstOrDefaultAsync(t => t.Host.Equals(hostName, StringComparison.InvariantCultureIgnoreCase), cancelationToken);
        }
    }
}
