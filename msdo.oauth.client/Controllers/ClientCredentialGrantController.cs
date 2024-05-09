using Microsoft.AspNetCore.Mvc;
using msdo.oauth.client.Interfaces;

namespace msdo.oauth.client.Controllers
{
    [Route("[controller]")]
    public class ClientCredentialGrantController : Controller
    {
        private IAuthorizationService _authorizationService;
        private IProtectedResourceService _protectedResourceService;
        private ILogger<ClientCredentialGrantController> _logger;

        public ClientCredentialGrantController(
            IAuthorizationService authorizationService, 
            IProtectedResourceService protectedResourceService,
            ILogger<ClientCredentialGrantController> logger)
        {
            _authorizationService = authorizationService;
            _protectedResourceService = protectedResourceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> UseClientCredentialGrantToAccessProtectedResource()
        {
            string correlationId = HttpContext.Items["Correlation-Id"].ToString();

            _logger.LogInformation("Requesting access token");

            string accessToken 
                = await _authorizationService.GetAccessToken("client", "secret",correlationId);

            _logger.LogInformation($"Obtained Access token: {accessToken}");

            _logger.LogInformation($"Requesting access to protected resource");

            string protectedResource
                = await _protectedResourceService.GetProtectedResource(accessToken, correlationId); 

            _logger.LogInformation($"Protected Resource retrieved: {protectedResource}");

            return Ok(protectedResource);


        }
    }
}
