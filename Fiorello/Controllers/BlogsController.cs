using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.ViewModels.Blogs;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Controllers
{
	public class BlogsController : Controller
	{
		private readonly AppDbContext _context;

		public BlogsController(AppDbContext context)
        {
			_context = context;
		}
        [HttpGet]
		public IActionResult Index()
		{
			var blogs = _context.Blogs.Where(b => !b.IsDeleted).ToList();

			var model = new BlogsIndexVM
			{
				Blogs = blogs
			};

			return View(model);
		}

		[HttpGet]
		public IActionResult Details(int id) 
		{
			var blog = _context.Blogs.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
			if (blog == null) return NotFound();

			var model = new BlogsDetailsVM
			{
				Blog = blog
			};

			return View(model);
		}
	}
}
