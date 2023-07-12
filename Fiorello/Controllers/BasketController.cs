using Fiorello.DAL;
using Fiorello.ViewModels.Basket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fiorello.Controllers
{
	public class BasketController : Controller
	{
		private readonly AppDbContext _context;

		public BasketController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
            List<BasketModel> basket;

            var basketCookie = Request.Cookies["basket"];
			if (basketCookie is null)
				basket = new List<BasketModel>();
			else
				basket = JsonConvert.DeserializeObject<List<BasketModel>>(basketCookie);

			foreach(var basketProduct in basket)
			{
				var product = await _context.Products.Include(p => p.ProductPhotos).FirstOrDefaultAsync(p => p.Id == basketProduct.Id);
				if (product != null)
				{
					basketProduct.Title = product.Title;
					basketProduct.Price = product.Price;
					basketProduct.StockQuantity = product.Stock;
					basketProduct.PhotoName = product.ProductPhotos.FirstOrDefault(p => p.IsMain).Name;
				}
			}

			return View(basket);
		}

		[HttpGet]
		public async Task<IActionResult> AddAsync(int id)
		{
			List<BasketModel> basket;

			var basketCookie = Request.Cookies["basket"];
			if (basketCookie == null)
				basket = new List<BasketModel>();
			else
				basket = JsonConvert.DeserializeObject<List<BasketModel>>(basketCookie);

			var product = await _context.Products.FindAsync(id);
			if (product is null)
				return NotFound("Product is not found");

			var basketProduct = basket.Find(bp => bp.Id == product.Id);
			if (basketProduct is not null)
			{
				if (product.Stock == basketProduct.Count)
					return NotFound("No more left at stock");

				basketProduct.Count++;
			}
			else
			{
				basket.Add(new BasketModel
				{
					Id = product.Id,
					Count = 1
				});
			}

			Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
			return Ok("Product successfully added to basket");
		}

		[HttpGet]
		public async Task<IActionResult> IncreaseCount(int id)
		{
			List<BasketModel> basket = JsonConvert.DeserializeObject<List<BasketModel>>(Request.Cookies["basket"]);

			var product = await _context.Products.FindAsync(id);
			if (product is null)
				return NotFound("Product is not found");

			var basketProduct = basket.Find(bp => bp.Id == product.Id);
			if (basketProduct is not null)
			{
				if (product.Stock == basketProduct.Count)
					return NotFound("No more left at stock");

				basketProduct.Count++;
			}

			Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> DecreaseCount(int id)
		{
			List<BasketModel> basket = JsonConvert.DeserializeObject<List<BasketModel>>(Request.Cookies["basket"]);

			var product = await _context.Products.FindAsync(id);
			if (product is null)
				return NotFound("Product is not found");

			var basketProduct = basket.Find(bp => bp.Id == product.Id);
			if (basketProduct is not null)
			{
				if (basketProduct.Count == 1)
					basket.Remove(basketProduct);

				basketProduct.Count--;
			}

			Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
			return RedirectToAction(nameof(Index));
		}
	}
}
