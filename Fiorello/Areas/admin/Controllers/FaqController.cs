using Fiorello.Areas.admin.ViewModels.Faq;
using Fiorello.DAL;
using Fiorello.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	public class FaqController : Controller
	{
		private readonly AppDbContext _context;

		public FaqController(AppDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult List()
		{
			var model = new FaqListVM
			{
				Faqs = _context.Faqs.Where(f => !f.IsDeleted).ToList(),
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult Create() 
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(FaqCreateVM model) 
		{
			if (!ModelState.IsValid) return View();

			var faq = new Faq
			{
				Title = model.Title,
				Description = model.Description,
				CreatedAt = DateTime.Now,
			};

			_context.Faqs.Add(faq);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Update(int id)
		{
			var faq = _context.Faqs.FirstOrDefault(f => f.Id == id && !f.IsDeleted);
			if (faq == null) return NotFound();

			var model = new FaqUpdateVM
			{
				Title = faq.Title,
				Description = faq.Description,
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Update(int id, FaqUpdateVM model)
		{
			if (!ModelState.IsValid) return View();

			var faq = _context.Faqs.FirstOrDefault(f => f.Title.Trim().ToLower() == model.Title.Trim().ToLower() && f.Id != id && !f.IsDeleted);

			if(faq is not null)
			{
				ModelState.AddModelError("Title", "Faq under this title already exists");
				return View(model);
			}

			faq = _context.Faqs.FirstOrDefault(f => f.Id == id && !f.IsDeleted);
			if (faq is null) return NotFound();	

			faq.Title = model.Title;
			faq.Description = model.Description;
			faq.ModifiedAt = DateTime.Now;

			_context.Faqs.Update(faq);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			var faq = _context.Faqs.FirstOrDefault(f => f.Id == id && !f.IsDeleted);
			if (faq is null) return NotFound();

			faq.IsDeleted = true;
			faq.DeletedAt = DateTime.Now;  

			_context.Faqs.Update(faq);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}
	}
}
