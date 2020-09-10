using System;
using System.Threading.Tasks;
using Authorization.DTO;
using Authorization.Manager;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Resources.Api.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class TenantController : Controller
    {
        private readonly ITenantManager _tenantManager;

        public TenantController(ITenantManager tenantManager)
        {
            _tenantManager = tenantManager;
        }

        [HttpGet]
        [ActionName("GetById")]
        public async Task<IActionResult> GetById(Guid tenantId)
        {
            var tenant = await _tenantManager.GetTenantById(tenantId);
            return Ok(new OkResponse(Mapper.Map<TenantDTO>(tenant), 1));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]TenantDTO tenant)
        {
            var createdTenant = await _tenantManager.Add(Mapper.Map<Tenant>(tenant));

            if (createdTenant.Item1 == null)
            {
                return BadRequest(new BadRequestResponse(ModelState, ExceptionKeyHelper.GetString(createdTenant.Item2.Value)));
            }
            return CreatedAtAction("GetById", new { id = createdTenant.Item1.Id }, new CreatedResponse(Mapper.Map<TenantDTO>(createdTenant.Item1), 1));
        }


    }
}
