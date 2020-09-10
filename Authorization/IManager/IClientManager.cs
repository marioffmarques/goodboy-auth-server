using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Authorization.Manager
{
    /// <summary>
    /// Client manager.
    /// Manages operations over IdentityServer4 Clients (application that can access the system)
    /// </summary>
    public interface IClientManager
    {
        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <returns>The clients.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<IEnumerable<Client>> GetClients(Guid tenantId, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Gets the client by identifier.
        /// </summary>
        /// <returns>The client by identifier.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="clientId">Client identifier.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<Client> GetClientById(Guid tenantId, string clientId, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Add the specified client and cancelationToken.
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="client">Client.</param>
        /// <param name="tenantId">TenantId.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<(Client, ExceptionKey?)> Add(Client client, Guid tenantId, CancellationToken cancelationToken = default(CancellationToken));
    }
}
