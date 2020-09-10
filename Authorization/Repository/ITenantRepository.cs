using System;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization.Repository
{
    /// <summary>
    /// Tenant repository. Gets Tenants from persisten storage
    /// </summary>
    public interface ITenantRepository : IRepository<Tenant>
    {
        /// <summary>
        /// Gets the tenant by hostname.
        /// </summary>
        /// <returns>The tenant by hostname.</returns>
        /// <param name="hostName">Host name.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<Tenant> GetTenantByHostname(string hostName, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Gets the name of the tenant by.
        /// </summary>
        /// <returns>The tenant by name.</returns>
        /// <param name="name">Name.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<Tenant> GetTenantByName(string name, CancellationToken cancelationToken = default(CancellationToken));
    }
}
