using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api.Controllers
{
    public class MultitenantBaseController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;

        public MultitenantBaseController(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }
            _httpContextAccessor = httpContextAccessor;
        }

        public Tenant GetContextTenant() 
        {
            return _httpContextAccessor?.HttpContext?.GetTenantContext<Tenant>()?.Tenant;
        }



    }
}
