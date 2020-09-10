using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using IdentityModel.Client;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            await CallApiUsingUserAccessToken();
            await CallApiUsingClientCredentials();

            return View();


        }

        [Authorize]
        public async Task<IActionResult> Secure()
        {
            ViewData["Message"] = "Secure page.";

           // await CallApiUsingClientCredentials();

            return View();
        }

        public async Task<IActionResult> CallApiUsingClientCredentials()
        {
            var tokenClient = new TokenClient("http://tenant2.localhost:5000/connect/token", "mvc-client8", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }


        public async Task Logout()
        {
            //return SignOut(new AuthenticationProperties()
            //{
            //    RedirectUri = "/Home/Index"
            //}, "Cookies", "oidc");

            await HttpContext.SignOutAsync("oidc");
            await HttpContext.SignOutAsync("Cookies");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}