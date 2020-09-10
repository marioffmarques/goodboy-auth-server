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
    public class ClientController : Controller
    {
        private IClientManager _clientManager;

        public ClientController(IClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        [HttpGet]
        [ActionName("GetById")]
        public async Task<IActionResult> GetById(string clientId, [FromQuery]Guid tenantId)
        {
            //var tenant = GetContextTenant();
            var client = await _clientManager.GetClientById(tenantId, clientId);
            return Ok(new OkResponse(Mapper.Map<ClientDTO>(client), 1));
        }


        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDTO client)
        {
            var createdClient = await _clientManager.Add(Mapper.Map<Client>(client), client.TenantId);

            if (createdClient.Item1 == null)
            {
                return BadRequest(new BadRequestResponse(ModelState, ExceptionKeyHelper.GetString(createdClient.Item2.Value)));
            }
            return CreatedAtAction("GetById", new { clientId = createdClient.Item1.ClientId, tenantId = client.TenantId }, new CreatedResponse(Mapper.Map<ClientDTO>(createdClient.Item1), 1));
        }
    }
}