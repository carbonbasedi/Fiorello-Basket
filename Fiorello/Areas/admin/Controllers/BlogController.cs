using Fiorello.Areas.admin.ViewModels.Blog;
using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.Utilities.File;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	public class BlogController : Controller
	{
		private readonly IFileService _fileService;
		private readonly AppDbContext _context;
        public BlogController(AppDbContext context, IFileService fileService)
        {
            _context = context;
			_fileService = fileService;
        }

		[HttpGet]
        public IActionResult List()
		{
			var model = new BlogListVM
			{
				Blogs = _context.Blogs.Where(b => !b.IsDeleted).ToList(),
			};
			return View(model);
		}

		[HttpGet]
		public IActionResult Create() 
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(BlogCreateVM model)
		{
			if (!ModelState.IsValid) return View();

			if (!_fileService.IsImage(model.Image))
			{
				ModelState.AddModelError("Photo", "Wrong file format");
				return View();
			}

			if (_fileService.IsBiggerThanSize(model.Image, 200))
			{
				ModelState.AddModelError("Photo", "File size is over 200kb");
				return View();
			}

			var blog = new Blog
			{
				Title = model.Title,
				Subtitle = model.Subtitle,
				About = model.About,
				Motto = model.Motto,
				About2 = model.About2,
				Motto2 = model.Motto2,
				About3 = model.About3,
				CreatedAt = DateTime.Now,
				Image = _fileService.Upload(model.Image)
			};

			_context.Blogs.Add(blog);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			var blog = _context.Blogs.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
			if (blog == null) NotFound();

			blog.IsDeleted = true;
			_context.Update(blog);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		[HttpGet]
		public IActionResult Update(int id)
		{
			var blog = _context.Blogs.FirstOrDefault(b =>b.Id == id && !b.IsDeleted);
			if (blog == null) NotFound();

			var model = new BlogUpdateVM
			{
				Title = blog.Title,
				Subtitle = blog.Subtitle,
				About = blog.About,
				Motto = blog.Motto,
				About2 = blog.About2,
				Motto2 = blog.Motto2,
				About3 = blog.About3,
				ImageUrl = blog.Image
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Update(int id, BlogUpdateVM model)
		{
			if(!ModelState.IsValid) return View();

			var blog = _context.Blogs.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
			if (blog == null) NotFound();

			if (model.Photo is not null)
			{
				if (_fileService.IsImage(model.Photo))
				{
					ModelState.AddModelError("Photo", "Wrong file format");
					return View();
				}

				if (_fileService.IsBiggerThanSize(model.Photo, 200))
				{
					ModelState.AddModelError("Photo", "File size is over 200kb");
					return View();
				}

				_fileService.Delete(blog.Image);
				blog.Image = _fileService.Upload(model.Photo);
			}

			blog.Title = model.Title;
			blog.Subtitle = model.Subtitle;
			blog.About = model.About;
			blog.About2 = model.About2;
			blog.About3 = model.About3;
			blog.Motto = model.Motto;
			blog.Motto2 = model.Motto2;
			blog.ModifiedAt = DateTime.Now;

			_context.Blogs.Update(blog);
			_context.SaveChanges();

			return RedirectToAction(nameof(List));
		}
	}
}
