using Azure.Core;
using Fiorello.Areas.admin.ViewModels.Duty;
using Fiorello.DAL;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	public class DutyController : Controller
	{
		private readonly AppDbContext _context;

		public DutyController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult List()
		{
			var model = new DutyIndexVM
			{
				Duties = _context.Duties.Where(d => !d.IsDeleted).ToList()
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(DutyCreateVM model)
		{
			if(!ModelState.IsValid) return View();

			var duty = _context.Duties.FirstOrDefault(d => d.Name.Trim().ToLower() == model.Name.Trim().ToLower() && !d.IsDeleted);

			if(duty is not null)
			{
				ModelState.AddModelError("Name", "Duty with such name already exists");
				return View();
			}

			duty = new Models.Duty
			{
				Name = model.Name,
				CreatedAt = DateTime.Now,
			};

			_context.Duties.Add(duty);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			var duty = _context.Duties.Include(d => d.Workers.Where(w => !w.IsDeleted)).FirstOrDefault(d => d.Id == id && !d.IsDeleted);

			if (duty is null) return NotFound();

			var model = new DutyDetailsVM
			{
				Name = duty.Name,
				CreatedAt = duty.CreatedAt,
				Workers = duty.Workers.ToList(),
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult Update(int id)
		{
			var duty = _context.Duties.FirstOrDefault(d => d.Id == id && !d.IsDeleted);
			if (duty is null) return NotFound();

			var model = new DutyUpdateVM
			{
				Name = duty.Name,
			};

			return View(model);
		}


		[HttpPost]
		public IActionResult Update(int id , DutyUpdateVM model)
		{
			if (!ModelState.IsValid) return View();

			var duty = _context.Duties.FirstOrDefault(d => d.Name.Trim().ToLower() == model.Name.Trim().ToLower() && d.Id != id && !d.IsDeleted);

			if (duty is not null)
			{
				ModelState.AddModelError("Name", "Duty under this name already exists");
				return View();
			}

			duty = _context.Duties.FirstOrDefault(d => d.Id == id && !d.IsDeleted);
			if (duty is null) return NotFound();

			duty.Name = model.Name;
			duty.ModifiedAt = DateTime.Now;

			_context.Duties.Update(duty);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Delete(int id) 
		{
			var duty = _context.Duties.FirstOrDefault(d => d.Id == id);
			if (duty is null) return NotFound();

			duty.IsDeleted = true;
			duty.DeletedAt = DateTime.Now;

			_context.Duties.Update(duty);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}
	}
}
