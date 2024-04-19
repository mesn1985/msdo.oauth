using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace msdo.oauth.client.Controllers
{
    [Route("[controller]")]
    public class ClientCredentialGrantController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var metaDataFromDiscoveryEndpoint = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (metaDataFromDiscoveryEndpoint.IsError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = metaDataFromDiscoveryEndpoint.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "protectedResource"
            });

            var protectedResourceServerHttpClient = new HttpClient();
            protectedResourceServerHttpClient.SetBearerToken(tokenResponse.AccessToken);
            var responseFromProtectedResourceServer =
                await protectedResourceServerHttpClient.GetAsync("https://localhost:5002/Identity");

            if (!responseFromProtectedResourceServer.IsSuccessStatusCode)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            string content = await responseFromProtectedResourceServer.Content.ReadAsStringAsync();

            return Ok(content);


        }
    }
}
