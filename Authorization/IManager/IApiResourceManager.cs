using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Authorization.Manager
{
    /// <summary>
    /// Interface ApiRessourceManager
    /// Manages operations over IdentityServer4 Api Ressources (Ressources that Clients can have access to)
    /// </summary>
    public interface IApiResourceManager
    {
        /// <summary>
        /// Gets the API resource by name
        /// </summary>
        /// <returns>The API resource.</returns>
        /// <param name="name">Name.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<ApiResource> GetApiResource(string name, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Add the specified apiRessource
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="apiRessource">API ressource.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<(ApiResource, ExceptionKey?)> Add(ApiResource apiRessource, CancellationToken cancelationToken = default(CancellationToken));
    }
}
