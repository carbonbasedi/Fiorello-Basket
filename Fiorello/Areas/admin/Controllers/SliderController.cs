using Fiorello.Areas.admin.ViewModels.Slider;
using Fiorello.DAL;
using Fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	public class SliderController : Controller
	{
		private readonly AppDbContext _context;
		public SliderController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult List()
		{
			var model = new SliderListVM
			{
				Sliders = _context.Sliders.Where(s => !s.IsDeleted).ToList()
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(SliderCreateVM model) 
		{
			if(!ModelState.IsValid) return View();

			var slider = new Slider
			{
				Title = model.Title,
				CreatedAt = DateTime.Now,
			};

			_context.Sliders.Add(slider);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			var slider = _context.Sliders.Include(s => s.SliderContents).FirstOrDefault(s => s.Id == id && !s.IsDeleted);
			if (slider == null) return NotFound();

			var model = new SliderDetailsVM
			{
				Title = slider.Title,
				CreatedAt = slider.CreatedAt,
				ModifiedAt = slider.ModifiedAt,
				SliderContents = slider.SliderContents.ToList(),
			};
			return View(model);
		}

		[HttpGet]
		public IActionResult Update(int id) 
		{
			var slider = _context.Sliders.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
			if (slider == null) return NotFound();

			var model = new SliderUpdateVM
			{
				Title = slider.Title,
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Update(int id, SliderUpdateVM model)
		{
			if (!ModelState.IsValid) return View();

			var slider = _context.Sliders.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
			if (slider == null) return NotFound();	

			slider.Title = model.Title;
			slider.ModifiedAt = DateTime.Now;

			_context.Sliders.Update(slider);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));	
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			var slider = _context.Sliders.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
			if (slider == null) return NotFound();

			slider.IsDeleted = true;
			slider.IsActive = false;
			slider.DeletedAt = DateTime.Now;

			_context.Sliders.Update(slider);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Activate(int id)
		{
			var slider = _context.Sliders.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
			if (slider == null) return NotFound();

			var dbSliders = _context.Sliders.Where(ds => ds.Id != slider.Id);

			if(!slider.IsActive)
			{
				foreach (var db in dbSliders)
				{
					db.IsActive = false;
					_context.Sliders.Update(db);
				}
				_context.SaveChanges();
			}

			slider.IsActive = !slider.IsActive;
			slider.ModifiedAt = DateTime.Now;

			_context.Sliders.Update(slider);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}
	}
}
