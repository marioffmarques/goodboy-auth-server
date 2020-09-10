using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Manager;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;

namespace Authorization.Api.MultiTenancy
{
    /// <summary>
    /// Client store resolver.
    /// MultitenantClientStoreResolver is intended to Resolve the IdentityServer Clients for a specific Tenant
    /// </summary>
    public class MultitenantClientStoreResolver : IClientStore
    {
        private readonly IClientManager clientManager;
        public Guid TenantId { get; set; }


        public MultitenantClientStoreResolver(IHttpContextAccessor httpContextAccessor, IClientManager clientManager)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }
            this.clientManager = clientManager;
            TenantId = httpContextAccessor.HttpContext?.GetTenantContext<Tenant>()?.Tenant?.Id ?? Guid.Empty;
        }


        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return clientManager.GetClientById(TenantId, clientId);
        }

    }
}