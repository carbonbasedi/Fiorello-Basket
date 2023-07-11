using Fiorello.Areas.admin.ViewModels.SliderContent;
using Fiorello.DAL;
using Fiorello.Utilities.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	public class SliderContentController : Controller
	{
		private readonly AppDbContext _context;

		public SliderContentController(AppDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult List()
		{
			var model = new SliderContentListVM
			{
				SliderContents = _context.SliderContents.Include(sc => sc.Slider).ToList()
			};
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			var model = new SliderContentCreateVM
			{
				SliderTitleList = _context.Sliders.Where(s => !s.IsDeleted).Select(s => new SelectListItem
				{
					Text = s.Title,
					Value = s.Id.ToString(),
				}).ToList(),
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Create(SliderContentCreateVM model)
		{
			model.SliderTitleList = _context.Sliders.Where(s => !s.IsDeleted).Select(s => new SelectListItem
			{
				Text = s.Title,
				Value = s.Id.ToString(),
			}).ToList();

			if (!ModelState.IsValid) return View(model);

			var content = _context.SliderContents.FirstOrDefault(c => c.Title.Trim().ToLower() == model.Title.Trim().ToLower());

			if (content is not null)
			{
				ModelState.AddModelError("Title", "There is no slider title under this name");
				return View(model);
			}

			content = new Models.SliderContent
			{
				Title = model.Title,
				About = model.About,
				SliderId = model.SliderId,
				CreatedAt = DateTime.Now,
			};

			_context.SliderContents.Add(content);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Update(int id)
		{
			var sliderContent = _context.SliderContents.FirstOrDefault(s => s.Id == id);
			if (sliderContent is null) return NotFound();

			var model = new SliderContentUpdateVM
			{
				Title = sliderContent.Title,
				About = sliderContent.About,
				SliderId = sliderContent.SliderId,
				SliderTitleList = _context.Sliders.Where(s => !s.IsDeleted).Select(s => new SelectListItem
				{
					Text = s.Title,
					Value = s.Id.ToString(),
				}).ToList(),
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Update(int id, SliderContentUpdateVM model)
		{
			if (!ModelState.IsValid)
			{
				model.SliderTitleList = _context.Sliders.Where(s => !s.IsDeleted).Select(s => new SelectListItem
				{
					Text = s.Title,
					Value = s.Id.ToString(),
				}).ToList();
				return View(model);
			}

			var sliderContent = _context.SliderContents.FirstOrDefault(sc => sc.Title.Trim().ToLower() == model.Title.Trim().ToLower() && sc.Id != id);

			if (sliderContent is not null)
			{
				ModelState.AddModelError("Title", "There is no slider title under this name");
				return View(model);
			}

			sliderContent = _context.SliderContents.FirstOrDefault(sc => sc.Id == id);
			if (sliderContent is null) return NotFound();

			var slider = _context.SliderContents.FirstOrDefault(sc => sc.Id == model.SliderId);
			if (slider == null)
			{
				ModelState.AddModelError("SliderId", "There is already Slider Title under this name");
			}

			sliderContent.Title = model.Title;
			sliderContent.About = model.About;
			sliderContent.SliderId = model.SliderId;
			sliderContent.ModifiedAt = DateTime.Now;

			_context.SliderContents.Update(sliderContent);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			var sliderContent = _context.SliderContents.Include(sc => sc.Slider).FirstOrDefault(sc => sc.Id == id);

			if (sliderContent is null) return NotFound();

			var model = new SliderContentDetailsVM
			{
				Title = sliderContent.Title,
				About = sliderContent.About,
				Slider = sliderContent.Slider,
				CreatedAt = sliderContent.CreatedAt,
				ModifiedAt = sliderContent.ModifiedAt,
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult Delete(int id) 
		{
			var sliderContent = _context.SliderContents.FirstOrDefault(sc => sc.Id == id);
			if(sliderContent is null) return NotFound();

			_context.SliderContents.Remove(sliderContent);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}
	}
}
