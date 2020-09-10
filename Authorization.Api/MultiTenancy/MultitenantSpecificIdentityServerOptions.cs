using System;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Http;

namespace Authorization.Api.MultiTenancy
{
    //public class MultitenantSpecificIdentityServerOptions : IdentityServerOptions
    //{
    //    public MultitenantSpecificIdentityServerOptions(IHttpContextAccessor httpContextAccessor)
    //    {
            
    //        if (httpContextAccessor == null)
    //        {
    //            throw new ArgumentNullException(nameof(httpContextAccessor));
    //        }
                
    //        var tenantContext = httpContextAccessor.HttpContext.GetTenantContext<Tenant>();
    //        if (tenantContext == null)
    //        {
    //            throw new ArgumentNullException(nameof(tenantContext));
    //        }

    //        // Try this if theres problems with diferent tenant cookies
    //        //this.Authentication.CheckSessionCookieName = "idsvr.tenants." + tenantContext.Tenant.Name;


    //        //TODO
    //        // we scope the IdSvr cookie with the tenant name, to avoid potential conflicts
    //        //AuthenticationOptions. = "idsvr.tenants." + tenantContext.Tenant.Name;
    //    }
    //}
}