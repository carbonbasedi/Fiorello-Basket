using Fiorello.DAL;
using Fiorello.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var slider = _context.Sliders.Include(s => s.SliderContents).FirstOrDefault(s => s.IsActive && !s.IsDeleted);

			var model = new HomeIndexVM
			{
				Slider = slider
			};

			return View(model);
		}
	}
}
