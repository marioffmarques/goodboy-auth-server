using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Repository;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;

namespace Authorization.IdentityServer4.Repository
{
    public class ApiResourceRepository : IApiResourceRepository
    {
        private readonly IConfigurationDbContext configurationDbContext;

        public ApiResourceRepository(ConfigurationDbContext context)
        {
            configurationDbContext = context;
        }

        public void Add(ApiResource entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            configurationDbContext.ApiResources.Add(entity.ToEntity());
        }

        public Task<int> Count(CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<int> Count(Expression<Func<ApiResource, bool>> clause, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Delete(ApiResource entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApiResource> FindAsync(Func<ApiResource, bool> clause, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiResource>> GetAllAsync(int? index, int? offset, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ApiResource> GetApiResource(string name, CancellationToken cancelationToken = default(CancellationToken))
        {
            var apiResource =
                configurationDbContext.ApiResources
                                      .Include(e => e.Scopes)
                                      .Include(e => e.Secrets)
                                      .Include(e => e.UserClaims)
                                      .Where(cl => cl.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                      .FirstOrDefault();

            return Task.FromResult<ApiResource>(apiResource?.ToModel());
        }

        public Task<ApiResource> GetAsync(object id, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Update(ApiResource entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<int> Commit(CancellationToken cancelationToken = default(CancellationToken))
        {
            return configurationDbContext.SaveChangesAsync();
        }
    }
}
