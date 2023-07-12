using Fiorello.DAL;
using Fiorello.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Controllers
{
	public class ProductsController : Controller
	{
		private readonly AppDbContext _context;

		public ProductsController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var model = new ProductsIndexVM
			{
				Products = _context.Products.Include(p => p.ProductPhotos).Where(p => !p.IsDeleted).OrderByDescending(p => p.Id).Take(4).ToList()
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult LoadMore(int skipRow)
		{
			var model = new ProductLoadMoreVM
			{
				Products = _context.Products.Include(p => p.ProductPhotos).Where(p => !p.IsDeleted).OrderByDescending(p => p.Id).Skip(4 * skipRow).Take(4).ToList()
			};
			return PartialView("_ProductComponentPartial", model);
		}

		[HttpGet]
		public IActionResult Details()
		{
			return View();
		}
	}
}
