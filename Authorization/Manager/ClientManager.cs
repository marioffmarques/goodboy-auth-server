using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Repository;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Authorization.Manager
{
    public class ClientManager : IClientManager
    {
        private readonly IClientRepository clientRepository;
        private readonly IConfiguration configuration;

        public ClientManager(IClientRepository clientRepository, IConfiguration configuration)
        {
            this.clientRepository = clientRepository;
            this.configuration = configuration;
        }

        public Task<Client> GetClientById(Guid tenantId, string clientId, CancellationToken cancelationToken = default(CancellationToken))
        {
            return clientRepository.GetByClientId(tenantId, clientId, cancelationToken);
        }

        public Task<IEnumerable<Client>> GetClients(Guid tenantId, CancellationToken cancelationToken = default(CancellationToken))
        {
            return clientRepository.GetClients(tenantId, cancelationToken);
        }

        public async Task<(Client, ExceptionKey?)> Add(Client client, Guid tenantId, CancellationToken cancelationToken = default(CancellationToken)) 
        {
            string potencialClientId = client.ClientId ?? client.ClientName.Slugify();

            if (await clientRepository.GetByClientId(tenantId, potencialClientId, cancelationToken) != null)
            {
                return (null, ExceptionKey.ERROR_IN_USE);
            }

            var clientSecret = Helpers.RandomKey.Generate(32);
            client.ClientId = potencialClientId;
            client.ClientSecrets = new [] { new Secret(clientSecret.Sha256()) };
            client.RequireConsent = false;
            client.AccessTokenLifetime = 60 * 60 * 24 * 7;
            client.ClientUri = String.Format("{0}://{1}.{2}", configuration["ClientsProtocol"], client.ClientId, configuration["ClientDomain"]);
            client.RedirectUris = new[] { String.Format("{0}://{1}.{2}/signin-oidc", configuration["ClientsProtocol"], client.ClientId, configuration["ClientDomain"]) };
            client.PostLogoutRedirectUris = new[] { String.Format("{0}://{1}.{2}/signout-callback-oidc", configuration["ClientsProtocol"], client.ClientId, configuration["ClientDomain"]) };

            if (client.AllowedScopes == null || client.AllowedScopes.Count == 0)
            {
                client.AllowedScopes = new[]
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                };
            }

            if (client.AllowedGrantTypes == null || client.AllowedGrantTypes.Count == 0)
            {
                client.AllowedGrantTypes = GrantTypes.HybridAndClientCredentials;
            }

            client.AllowOfflineAccess = true;
            client.Properties = new Dictionary<string, string> { { "TenantId", tenantId.ToString() } };


            clientRepository.Add(client, cancelationToken);

            if (await clientRepository.Commit(cancelationToken) > 0)
            {
                client.ClientSecrets = new[] { new Secret(clientSecret) };
                return (client, null);
            }
            else
            {
                return (null, ExceptionKey.ERROR_CREATE);
            }
        }
    }
}
