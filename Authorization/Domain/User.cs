using System;
using Microsoft.AspNetCore.Identity;

namespace Authorization
{
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>The tenant identifier.</value>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Gets or sets the tenant.
        /// </summary>
        /// <value>The tenant.</value>
        public Tenant Tenant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Authorization.User"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }



        public string GetUsername(string tenantName, string email)
        {
            return String.Format("{0}-{1}", tenantName, email);
        }
    }
}
