using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Authorization.Repository
{
    /// <summary>
    /// Client repository. Gets clients from persistent storage
    /// </summary>
    public interface IClientRepository : IRepository<Client>
    {
        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <returns>The clients.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<IEnumerable<Client>> GetClients(Guid tenantId, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Gets the by client identifier.
        /// </summary>
        /// <returns>The by client identifier.</returns>
        /// <param name="tenantId">Tenant identifier.</param>
        /// <param name="clientId">Client identifier.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<Client> GetByClientId(Guid tenantId, string clientId, CancellationToken cancelationToken = default(CancellationToken));


        //public void Add(Client entity, CancellationToken cancelationToken = default(CancellationToken))
    }
}
