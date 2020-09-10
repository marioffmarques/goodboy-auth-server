using System;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization.Manager
{
    /// <summary>
    /// Tenant manager. Provides a facade allowed tenant operations
    /// </summary>
    public interface ITenantManager
    {
        /// <summary>
        /// Gets the tenant by identifier.
        /// </summary>
        /// <returns>The tenant by identifier.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<Tenant> GetTenantById(Guid tenantId, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Gets the name of the tenant by.
        /// </summary>
        /// <returns>The tenant by name.</returns>
        /// <param name="tenantName">Tenant name.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<Tenant> GetTenantByName(string tenantName, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Gets the tenant by hostname.
        /// </summary>
        /// <returns>The tenant by hostname.</returns>
        /// <param name="hostname">Hostname.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<Tenant> GetTenantByHostname(string hostname, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Add the specified tenant and cancelationToken.
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="tenant">Tenant.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<(Tenant, ExceptionKey?)> Add(Tenant tenant, CancellationToken cancelationToken = default(CancellationToken));
    }
}
