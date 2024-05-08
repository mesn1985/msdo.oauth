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
        public async Task<IActionResult> Index()
        {
            string correlationId = HttpContext.Items["Correlation-Id"].ToString();

            _logger.LogInformation("Requesting access token");

            string accessToken 
                = await _authorizationService.GetAccessToken("client", "secret",correlationId);

            _logger.LogInformation($"Obtained Access token: {accessToken}");

            string protectedResource
                = await _protectedResourceService.GetProtectedResource(accessToken, correlationId); 

            return Ok(protectedResource);


        }
    }
}
