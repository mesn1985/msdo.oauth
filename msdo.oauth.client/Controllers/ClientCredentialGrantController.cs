using Microsoft.AspNetCore.Mvc;
using msdo.oauth.client.Interfaces;

namespace msdo.oauth.client.Controllers
{
    /// <summary>
    /// Client credential controller.
    /// This controller provides the endpoint used for invoking request for
    /// protected resource, where the client credential authorization grant is used to obtain access token
    /// @author: Martin Edwin Schjødt Nielsen
    /// </summary>
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
        /// <summary>
        ///  Provides a protected resource if a valid token is provided
        /// </summary>
        /// <param></param>
        /// <response code="200">Proteced resource have been obtained</response>
        [HttpGet]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
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
