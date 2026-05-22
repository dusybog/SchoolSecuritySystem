using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolSecuritySystem.Web.Controllers.Web
{
    [Route("setting")]
    [Authorize]
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
