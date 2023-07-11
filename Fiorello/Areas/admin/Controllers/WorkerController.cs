using Fiorello.Areas.admin.ViewModels.Worker;
using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.Utilities.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.admin.Controllers
{
    [Area("admin")]
    public class WorkerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public WorkerController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult List()
        {

            var model = new WorkerListVM
            {
                Workers = _context.Workers.Include(w => w.Duty).Where(w => !w.IsDeleted).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new WorkerCreateVM
            {
                DutyTitles = _context.Duties.Where(d => !d.IsDeleted).Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString(),
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(WorkerCreateVM model) 
        {
            model.DutyTitles = _context.Duties.Where(d => !d.IsDeleted).Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString(),
            }).ToList();

            if(!ModelState.IsValid) return View(model);

			if (!_fileService.IsImage(model.Photo))
			{
				ModelState.AddModelError("Photo", "Wrong file format");
				return View();
			}

			if (_fileService.IsBiggerThanSize(model.Photo, 200))
			{
				ModelState.AddModelError("Photo", "File size is over 200kb");
				return View();
			}

			var dutyTitle = _context.Duties.Find(model.DutyId);
            if (dutyTitle is null)
            {
                ModelState.AddModelError("DutyTitles", "Duty under this name doesn't exist");
                return View(model);
            }

            var worker = new Worker
            {
                Name = model.Name,
                Surname = model.Surname,
                Photo = _fileService.Upload(model.Photo),
                DutyId = model.DutyId,
                CreatedAt = DateTime.Now,
            };

            _context.Workers.Add(worker);
            _context.SaveChanges();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var worker = _context.Workers.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
            if (worker == null) return NotFound();

            _fileService.Delete(worker.Photo);
            worker.IsDeleted = true;
            worker.DeletedAt = DateTime.Now;

            _context.Workers.Update(worker);
            _context.SaveChanges();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult Update(int id) 
        {
            var worker = _context.Workers.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
            if (worker == null) return NotFound();

            var model = new WorkerUpdateVM
            {
                Name = worker.Name,
                Surname = worker.Surname,
                PhotoName = worker.Photo,
                DutyId= worker.DutyId,
                DutyTitles = _context.Duties.Where(d => !d.IsDeleted).Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString(),
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(int id,WorkerUpdateVM model)
        {
            if(!ModelState.IsValid)
            {
                model.DutyTitles = _context.Duties.Where(d => !d.IsDeleted).Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString(),
                }).ToList();

                return View(model);
			}

            var worker = _context.Workers.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
            if (worker == null) return NotFound();

            if(model.Photo is not null)
            {
				if (!_fileService.IsImage(model.Photo))
				{
					ModelState.AddModelError("Photo", "Wrong file format");
					return View();
				}

				if (_fileService.IsBiggerThanSize(model.Photo, 200))
				{
					ModelState.AddModelError("Photo", "File size is over 200kb");
					return View();
				}
				_fileService.Delete(worker.Photo);
				worker.Photo = _fileService.Upload(model.Photo);
			}

            worker.Name = model.Name;
            worker.Surname = model.Surname;
            worker.DutyId = model.DutyId;
            worker.ModifiedAt = DateTime.Now;

            _context.Workers.Update(worker);
            _context.SaveChanges();

            return RedirectToAction(nameof(List));
		}

        [HttpGet]
        public IActionResult Details(int id)
        {
            var worker = _context.Workers.Include(w => w.Duty).FirstOrDefault(w => w.Id == id && !w.IsDeleted);

            if (worker == null) return NotFound();

            var model = new WorkerDetailsVM
            {
                Name = worker.Name,
                Surname = worker.Surname,
                Duty = worker.Duty,
                ModifiedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Photo = worker.Photo,
            };

            return View(model);
        }
    }
}
