using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Authorization.DTO;
using System.Threading.Tasks;
using AutoMapper;
using Authorization.Manager;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.AccessTokenValidation;

namespace Authorization.Resources.Api.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ITenantManager _tenantManager;

        public UserController(UserManager<User> userManager, ITenantManager tenantManager)
        {
            _userManager = userManager;
            _tenantManager = tenantManager;
        }

        [HttpGet("{userId:guid}")]
        [ActionName("GetById")]
        public IActionResult GetById(Guid userId)
        {
            //var tenant = GetContextTenant();
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            return Ok(new OkResponse(Mapper.Map<UserDTO>(user), 1));
        }



        /// <summary>
        /// Register the specified user.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="user">User.</param>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserDTO user)
        {
            var role = user.Role ?? "User";
            var identityUser = Mapper.Map<User>(user);

            var tenant = await _tenantManager.GetTenantById(user.TenantId);

            if (tenant == null)
            {
                return BadRequest(new BadRequestResponse(null, ExceptionKeyHelper.GetString(ExceptionKey.ERROR_CREATE)));
            }


            identityUser.UserName = identityUser.GetUsername(tenant.Name, user.Email);

            var userResult = await _userManager.CreateAsync(identityUser, user.Password);
            if (userResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, role);

                var claims = new List<Claim>();
                claims.Add(new Claim(JwtClaimTypes.Name, user.Name));
                claims.Add(new Claim("tenant", tenant.Name));
                //claims.Add(new Claim(JwtClaimTypes..ClientId, user.Name));
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
                claims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
                claims.Add(new Claim(JwtClaimTypes.Role, role));

                await _userManager.AddClaimsAsync(identityUser, claims);

                return CreatedAtAction("GetById", new { userId = user.Id }, new CreatedResponse(Mapper.Map<UserDTO>(identityUser), 1));
            }
            return BadRequest(new BadRequestResponse(null, ExceptionKeyHelper.GetString(ExceptionKey.ERROR_CREATE)));
        }
    }
}
