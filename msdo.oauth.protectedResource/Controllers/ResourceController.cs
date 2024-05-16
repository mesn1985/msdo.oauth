using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace msdo.oauth.client.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ResourceController : Controller
    {
        private ILogger<ResourceController> _logger;
        
        public ResourceController(ILogger<ResourceController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        ///  Request access to a protected resource using the client credential grant.
        ///  Initiates the token flow for access to a protected resource.
        /// </summary>
        /// <param></param>
        /// <response code="200">Proteced resource have been obtained</response>
        ///  <response code="401">Invalid access token provided</response>
        // Identity framework handles the validation of the provided JWS (See configuration in Program.cs)
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public IActionResult Index()
        {
            string accessToken = GetAccessTokenFromHeader();
            //Never log keys!!!! It is only allowable here, because it is an educational tool
            _logger.LogInformation($"Authorized Request for protected resource was made with token: {accessToken}");

            return new JsonResult( "{ProtectedResource:ValueOfProtecedResource}");
        }

        private string GetAccessTokenFromHeader()
        {
            Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValue);

            return authorizationHeaderValue.ToString().Split().Skip(1).FirstOrDefault();
        }
    }
}
