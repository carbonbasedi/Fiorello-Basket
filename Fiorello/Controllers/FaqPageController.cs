using Fiorello.DAL;
using Fiorello.ViewModels.FaqPage;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Controllers
{
	public class FaqPageController : Controller
	{
		private readonly AppDbContext _context;

		public FaqPageController(AppDbContext context)
        {
			_context = context;
		}
        public IActionResult Index()
		{
			var faqs = _context.Faqs.Where(x => !x.IsDeleted).ToList();

			var model = new FaqPageIndexVM
			{
				Faqs = faqs,
			};

			return View(model);
		}
	}
}
