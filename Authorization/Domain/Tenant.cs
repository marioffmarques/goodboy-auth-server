using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Authorization
{
    public class Tenant
    {
        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        /// <value>The tenant identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tenant.
        /// </summary>
        /// <value>The name of the tenant.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Authorization.Domain.Tenant"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the hosts.
        /// </summary>
        /// <value>The hosts.</value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Authorization.Domain.Tenant"/> is deleted.
        /// </summary>
        /// <value><c>true</c> if is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The tokens.</value>
        public ICollection<User> Users { get; set; }
    }
}
