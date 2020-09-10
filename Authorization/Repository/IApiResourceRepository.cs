using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Authorization.Repository
{
    public interface IApiResourceRepository : IRepository<ApiResource>
    {
        /// <summary>
        /// Gets the API resource by name
        /// </summary>
        /// <returns>The API resource.</returns>
        /// <param name="name">Name.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<ApiResource> GetApiResource(string name, CancellationToken cancelationToken = default(CancellationToken));


    }
}
