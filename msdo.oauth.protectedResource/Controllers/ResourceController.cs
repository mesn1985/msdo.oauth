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

        [HttpGet]
        public IActionResult Index()
        {
            string accessToken = GetAccessTokenFromHeader();

            _logger.LogInformation($"Request for protected resource was made with token: {accessToken}");

            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        private string GetAccessTokenFromHeader()
        {
            Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValue);

            return authorizationHeaderValue.ToString().Split().Skip(1).FirstOrDefault();
        }
    }
}
