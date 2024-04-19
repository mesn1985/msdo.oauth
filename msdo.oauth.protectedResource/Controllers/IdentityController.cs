using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace msdo.oauth.client.Controllers
{
    [Route("[controller]")]
    [Authorize]

    public class IdentityController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
