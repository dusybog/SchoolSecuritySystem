using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolSecuritySystem.Web.Controllers.Web
{
    [Authorize]
    public class SubmissionController : Controller
    {
        [HttpGet("/")]
        [HttpGet("submission")]
        public IActionResult Create()
        {
            // 對應 Views/Submission/create.cshtml
            return View();
        }

        [HttpGet("submissions")]
        public IActionResult Index()
        {
            // 對應 Views/Submission/index.cshtml
            return View();
        }

        [HttpGet("submission/{id:long}")]
        public IActionResult Detail(long id)
        {
            ViewBag.SubmissionId = id;
            // 對應 Views/Submission/detail.cshtml
            return View();
        }
    }
}