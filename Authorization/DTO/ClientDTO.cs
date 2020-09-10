using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Authorization.DTO
{
    public class ClientDTO
    {
        
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string ClientUri { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        public ICollection<string> AllowedScopes { get; set; }  // Scopes (resources) that this client has access to

        public ICollection<string> AllowedGrantTypes { get; set; }     // Authentication types allowed for this client
    }
}
