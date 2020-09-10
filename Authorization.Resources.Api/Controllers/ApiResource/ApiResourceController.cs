using System;
using System.Threading.Tasks;
using Authorization.DTO;
using Authorization.Manager;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Resources.Api.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class ApiResourceController : Controller
    {
        private IApiResourceManager _apiResourceManager;

        public ApiResourceController(IApiResourceManager apiResourceManager)
        {
            _apiResourceManager = apiResourceManager;
        }

        [HttpGet]
        [ActionName("GetByName")]
        public async Task<IActionResult> GetByName(string apiRessourceName)
        {
            var apiResource = await _apiResourceManager.GetApiResource(apiRessourceName);
            return Ok(new OkResponse(Mapper.Map<ApiResourceDTO>(apiResource), 1));
        }


        [HttpPost]
        public async Task<IActionResult> CreateApiResource([FromBody] ApiResourceDTO apiResource)
        {
            var createdResource = await _apiResourceManager.Add(Mapper.Map<ApiResource>(apiResource));

            if (createdResource.Item1 == null)
            {
                return BadRequest(new BadRequestResponse(ModelState, ExceptionKeyHelper.GetString(createdResource.Item2.Value)));
            }
            return CreatedAtAction("GetByName", new { apiRessourceName = createdResource.Item1.Name }, new CreatedResponse(Mapper.Map<ApiResourceDTO>(createdResource.Item1), 1));
        }
    }
}
