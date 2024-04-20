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

            string accessToken 
                = await _authorizationService.GetAccessToken("client", "secret");

            string protectedResource
                = await _protectedResourceService.GetProtectedResource(accessToken); 

            return Ok(protectedResource);


        }
    }
}
