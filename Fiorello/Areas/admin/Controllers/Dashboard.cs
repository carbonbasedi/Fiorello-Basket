using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Areas.admin.Controllers
{
    public class Dashboard : Controller
    {
        [Area("admin")]
        [Authorize(Roles ="SuperAdmin, Admin, Manager, HR")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
