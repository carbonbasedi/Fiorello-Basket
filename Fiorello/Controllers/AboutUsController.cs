using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Controllers
{
	[Authorize]
	public class AboutUsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
