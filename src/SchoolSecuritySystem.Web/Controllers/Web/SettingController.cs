using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSecuritySystem.Core.Constants;

namespace SchoolSecuritySystem.Web.Controllers.Web
{
    [Route("setting")]
    [Authorize]
    public class SettingController : Controller
    {
        [Authorize(Roles = AppRoles.Center)] // 限制僅校安中心權限可修改
        public IActionResult Index()
        {
            return View();
        }
    }
}
