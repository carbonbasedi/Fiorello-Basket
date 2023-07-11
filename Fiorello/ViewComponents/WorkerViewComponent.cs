using Fiorello.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.ViewComponents
{
	public class WorkerViewComponent : ViewComponent
	{
		private readonly AppDbContext _context;

		public WorkerViewComponent(AppDbContext context)
        {
			_context = context;
		}
        public IViewComponentResult Invoke()
		{
			var workers = _context.Workers.Include(w => w.Duty).Where(w => !w.IsDeleted).ToList();
			return View(workers);
		}
	}
}
