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
    public class ClientRepository : IClientRepository
    {
        private readonly IConfigurationDbContext configurationDbContext;

        public ClientRepository(ConfigurationDbContext context)
        {
            configurationDbContext = context;
        }

        public void Add(Client entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            configurationDbContext.Clients.Add(entity.ToEntity());
        }

        public Task<int> Count(CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<int> Count(Expression<Func<Client, bool>> clause, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Delete(Client entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> FindAsync(Func<Client, bool> clause, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Client>> GetAllAsync(int? index, int? offset, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Client> GetAsync(object id, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Update(Client entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Client> GetByClientId(Guid tenantId, string clientId, CancellationToken cancelationToken = default(CancellationToken))
        {
            var client =
                 configurationDbContext.Clients
                             .Include(e => e.Claims)
                             .Include(e => e.AllowedCorsOrigins)
                             .Include(e => e.AllowedGrantTypes)
                             .Include(e => e.PostLogoutRedirectUris)
                             .Include(e => e.Properties)
                             .Include(e => e.RedirectUris)
                             .Include(e => e.ClientSecrets)
                             .Include(e => e.AllowedScopes)
                       .Where(cl => cl.ClientId.Equals(clientId, StringComparison.InvariantCultureIgnoreCase) && cl.Properties.Find(e => e.Key == "TenantId").Value == tenantId.ToString())
                       .FirstOrDefault();

            return Task.FromResult<Client>(client?.ToModel());
        }

        public Task<IEnumerable<Client>> GetClients(Guid tenantId, CancellationToken cancelationToken = default(CancellationToken))
        {
            var client = configurationDbContext.Clients
                             .Include(e => e.Claims)
                             .Include(e => e.AllowedCorsOrigins)
                             .Include(e => e.AllowedGrantTypes)
                             .Include(e => e.PostLogoutRedirectUris)
                             .Include(e => e.Properties)
                             .Include(e => e.RedirectUris)
                             .Include(e => e.ClientSecrets)
                             .Include(e => e.AllowedScopes)
                             .Where(cl => cl.Properties.Find(e => e.Key == "TenantId").Value == tenantId.ToString())
                             .Select(x => x.ToModel());

            return Task.FromResult<IEnumerable<Client>>(client);
        }

        public Task<int> Commit(CancellationToken cancelationToken = default(CancellationToken))
        {
            return configurationDbContext.SaveChangesAsync();
        }
    }
}
