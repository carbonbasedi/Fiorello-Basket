using Fiorello.Areas.admin.ViewModels.ProductCategory;
using Fiorello.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	public class ProductCategoryController : Controller
	{
		private readonly AppDbContext _context;

		public ProductCategoryController(AppDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult Index()
		{
			var model = new ProductCategoryIndexVM
			{
				ProductCategories = _context.ProductCategories.Where(pc => !pc.IsDeleted).ToList(),
			};
			return View(model);
		}

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(ProductCategoryCreateVM model)
		{
			if (!ModelState.IsValid) return View();

			var productCategory = _context.ProductCategories.FirstOrDefault(pc => pc.Name.Trim().ToLower() == model.Name.ToLower() && !pc.IsDeleted);
			if (productCategory is not null)
			{
				ModelState.AddModelError("Name", "Category under this name already exists in database");
				return View();
			}

			productCategory = new Models.ProductCategory
			{
				Name = model.Name,
				CreatedAt = DateTime.Now,
			};

			_context.ProductCategories.Add(productCategory);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
		#endregion

		#region Details
		public IActionResult Details(int Id)
		{
			var productCategory = _context.ProductCategories.Include(pc => pc.Products.Where(p => !p.IsDeleted))
															.FirstOrDefault(pc => pc.Id == Id && !pc.IsDeleted);

			if (productCategory is null) return NotFound();

			var model = new ProductCategoryDetailsVM
			{
				Name = productCategory.Name,
				IsDeleted = productCategory.IsDeleted,
				CreatedAt = productCategory.CreatedAt,
				ModifiedAt = productCategory.ModifiedAt,
				DeletedAt = productCategory.DeletedAt,
				Products = productCategory.Products.ToList()
			};

			return View(model);
		}
		#endregion

		#region Update
		[HttpGet]
		public IActionResult Update(int Id)
		{
			var productCategory = _context.ProductCategories.FirstOrDefault(pc => pc.Id == Id && !pc.IsDeleted);
			if (productCategory is null) return NotFound();

			var model = new ProductCategoryUpdateVM
			{
				Name = productCategory.Name,
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult Update(int Id, ProductCategoryUpdateVM model)
		{
			if (!ModelState.IsValid) return View();

			var productCategory = _context.ProductCategories.FirstOrDefault(wc => wc.Name.Trim().ToLower() == model.Name.Trim().ToLower() && wc.Id != Id && !wc.IsDeleted);
			if (productCategory is not null)
			{
				ModelState.AddModelError("Name", "Category under this name already exists in database");
				return View();
			}

			productCategory = _context.ProductCategories.FirstOrDefault(wc => wc.Id == Id && !wc.IsDeleted);
			if (productCategory is null) return NotFound();

			productCategory.Name = model.Name;
			productCategory.ModifiedAt = DateTime.Now;

			_context.ProductCategories.Update(productCategory);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
		#endregion

		#region Delete
		[HttpGet]
		public IActionResult Delete(int Id)
		{
			var productCategory = _context.ProductCategories.FirstOrDefault(pc => pc.Id == Id);
			if (productCategory is null) return NotFound();

			productCategory.IsDeleted = true;
			productCategory.DeletedAt = DateTime.Now;

			_context.ProductCategories.Update(productCategory);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
		#endregion
	}
}
