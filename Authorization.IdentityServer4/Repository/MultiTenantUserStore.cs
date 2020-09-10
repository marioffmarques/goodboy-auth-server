using System;
using System.Threading;
using System.Threading.Tasks;
using Authorization.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization.IdentityServer4.Repository
{
    /// <summary>
    /// Multi tenant user store.
    /// Intended to override the base implementation of UserStore in order to add TenantId Property
    /// Allowing to get User by email and username taking in account their TenantId
    /// </summary>
    public class MultiTenantUserStore<TUser> : UserStore<TUser, Role, DbContext, Guid> where TUser : User
    {
        public Guid TenantId { get; set; }

        public MultiTenantUserStore(AuthorizationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context) 
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            TenantId = httpContextAccessor.HttpContext?.GetTenantContext<Tenant>()?.Tenant?.Id ?? Guid.Empty;
        }

        public override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.TenantId = this.TenantId;
            return base.CreateAsync(user, cancellationToken);
        }

        public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Users.FirstOrDefaultAsync<TUser>(u => u.NormalizedEmail == normalizedEmail && u.TenantId == this.TenantId);
        }

        public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Users.FirstOrDefaultAsync<TUser>(u => u.NormalizedUserName == normalizedUserName && u.TenantId == this.TenantId);
        }
    }
}