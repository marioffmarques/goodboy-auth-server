using System;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Repository;
using IdentityServer4.Models;

namespace Authorization.Manager
{
    public class ApiResourceManager : IApiResourceManager
    {
        private readonly IApiResourceRepository apiRessourceRepository;

        public ApiResourceManager(IApiResourceRepository apiRessourceRepository)
        {
            this.apiRessourceRepository = apiRessourceRepository;
        }


        public Task<ApiResource> GetApiResource(string name, CancellationToken cancelationToken = default(CancellationToken))
        {
            return apiRessourceRepository.GetApiResource(name, cancelationToken);
        }


        public async Task<(ApiResource, ExceptionKey?)> Add(ApiResource apiRessource, CancellationToken cancelationToken = default(CancellationToken))
        {
            apiRessourceRepository.Add(apiRessource, cancelationToken);

            if (await apiRessourceRepository.Commit(cancelationToken) > 0)
            {
                return (apiRessource, null);
            }
            else
            {
                return (null, ExceptionKey.ERROR_CREATE);
            }
        }
    }
}
