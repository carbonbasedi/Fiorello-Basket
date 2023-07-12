using Fiorello.Areas.admin.ViewModels.Product;
using Fiorello.DAL;
using Fiorello.Enums;
using Fiorello.Models;
using Fiorello.Utilities.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Fiorello.Areas.admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public ProductController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ProductIndexVM
            {
                Products = _context.Products.Include(p => p.ProductCategory).Include(p => p.ProductPhotos).Where(p => !p.IsDeleted).ToList()
            };

            return View(model);
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductCreateVM
            {
                ProductCategories = _context.ProductCategories.Where(pc => !pc.IsDeleted).Select(pc => new SelectListItem
                {
                    Text = pc.Name,
                    Value = pc.Id.ToString(),
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProductCreateVM model)
        {

            model.ProductCategories = _context.ProductCategories.Where(pc => !pc.IsDeleted).Select(pc => new SelectListItem
            {
                Text = pc.Name,
                Value = pc.Id.ToString(),
            }).ToList();

            if (!ModelState.IsValid) return View(model);

            var product = _context.Products.FirstOrDefault(p => p.Title.Trim().ToLower() == model.Title.Trim().ToLower() &&
                                                            !p.IsDeleted);

            if (product is not null)
            {
                ModelState.AddModelError("Title", "There already exists product under this name");
                return View(model);
            }

            var productCategory = _context.ProductCategories.Find(model.ProductCategoryId);
            if (productCategory is null)
            {
                ModelState.AddModelError("ProductCategoryId", "Category under this name doesn's exits");
                return View(model);
            }

            foreach (var photo in model.Photos)
            {
                if (!_fileService.IsImage(photo))
                {
                    ModelState.AddModelError("Photos", "Wrong file format");
                    return View();
                }

                if (_fileService.IsBiggerThanSize(photo, 200))
                {
                    ModelState.AddModelError("Photos", "File size is over 200kb");
                    return View();
                }
            }

            product = new Models.Product
            {
                Title = model.Title,
                Price = model.Price,
                Stock = model.Stock,
                About = model.About,
                Type = model.Type,
                Description = model.Description,
                AdditionalInformation = model.AdditionalInfo,
                ProductCategoryId = model.ProductCategoryId,
                CreatedAt = DateTime.Now,
            };
            _context.Products.Add(product);
            int order = 1;
            foreach (var photo in model.Photos)
            {
                var productPhoto = new ProductPhoto
                {
                    Name = _fileService.Upload(photo),
                    Order = order++,
                    CreatedAt = DateTime.Now,
                    Product = product,
                };
                _context.ProductPhotos.Add(productPhoto);
            }

            product.ProductPhotos.FirstOrDefault().IsMain = true;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products.Include(p => p.ProductPhotos).Include(p => p.ProductCategory).FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (product is null) return NotFound();

            var model = new ProductDetailsVM
            {
                Title = product.Title,
                Price = product.Price,
                Type = product.Type,
                CreatedAt = product.CreatedAt,
                ModifiedAt = product.ModifiedAt,
                ProductCategory = product.ProductCategory,
                Photos = product.ProductPhotos.ToList(),
            };

            return View(model);
        }
        #endregion

        #region Delete 
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Include(p => p.ProductPhotos.Where(p => !p.IsMain)).FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (product is null) return NotFound();

            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;
            _context.Products.Update(product);

            if (product.ProductPhotos is not null)
            {
                foreach (var photo in product.ProductPhotos)
                {
                    _fileService.Delete(photo.Name);
                    _context.ProductPhotos.Remove(photo);
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Include(p => p.ProductCategory)
                                            .Include(p => p.ProductPhotos)
                                            .FirstOrDefault(p => p.Id == id && !p.IsDeleted);

            if (product is null) return NotFound();

            var model = new ProductUpdateVM
            {
                Title = product.Title,
                Price = product.Price,
                About = product.About,
                Stock = product.Stock,
                Type = product.Type,
                Description = product.Description,
                AdditionalInfo = product.AdditionalInformation,
                ProductCategories = _context.ProductCategories.Where(pc => !pc.IsDeleted).Select(pc => new SelectListItem
                {
                    Text = pc.Name,
                    Value = pc.Id.ToString(),
                }).ToList(),
                ProductCategoryId = product.ProductCategoryId,
                ProductPhotos = product.ProductPhotos.ToList(),
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(int id, ProductUpdateVM model)
        {

            if (!ModelState.IsValid)
            {

                model.ProductPhotos = _context.ProductPhotos.Where(pp => pp.ProductId == id).ToList();

                model.ProductCategories = _context.ProductCategories.Where(pc => !pc.IsDeleted).Select(pc => new SelectListItem
                {
                    Text = pc.Name,
                    Value = pc.Id.ToString(),
                }).ToList();

                return View(model);
            }

            var product = _context.Products.FirstOrDefault(p => p.Id != id &&
                                                            !p.IsDeleted &&
                                                            p.Title.Trim().ToLower() == model.Title.Trim().ToLower());

            if (product is not null)
            {
                ModelState.AddModelError("Name", "Product under this name exists");
                return View(model);
            }

            product = _context.Products.Include(p => p.ProductCategory).Include(p => p.ProductPhotos).FirstOrDefault(p => p.Id == id &&
                                                                                                                        !p.IsDeleted);
            if (product is null) return NotFound();

            var productCategory = _context.ProductCategories.FirstOrDefault(pc => !pc.IsDeleted && pc.Id == model.ProductCategoryId);
            if (productCategory is null)
            {
                ModelState.AddModelError("ProductCategory", "There is no Category under this name");
                return View(model);
            }

            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.About = model.About;
            product.Type = model.Type;
            product.AdditionalInformation = model.AdditionalInfo;
            product.ProductCategoryId = productCategory.Id;
            product.ProductCategory = productCategory;
            product.ModifiedAt = DateTime.Now;

            _context.Products.Update(product);

            foreach (var photo in model.Photos)
            {
                if (!_fileService.IsImage(photo))
                {
                    ModelState.AddModelError("Photos", "Wrong file format");
                    return View();
                }

                if (_fileService.IsBiggerThanSize(photo, 200))
                {
                    ModelState.AddModelError("Photos", "File size is over 200kb");
                    return View();
                }
            }

            var lastOrder = product.ProductPhotos.OrderByDescending(p => p.Order).FirstOrDefault()?.Order;
            int order = 1;
            foreach (var photo in model.Photos)
            {
                var productPhoto = new ProductPhoto
                {
                    Name = _fileService.Upload(photo),
                    CreatedAt = DateTime.Now,
                    Product = product,
                    Order = lastOrder is not null ? (int)(++lastOrder) : order++
                };

                _context.ProductPhotos.Add(productPhoto);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), "product", new { id = product.Id });
        }

        [HttpGet]
        public IActionResult UpdatePhoto(int id)
        {
            var productPhoto = _context.ProductPhotos.Find(id);
            if (productPhoto is null) return NotFound();

            var model = new ProductPhotoUpdateVM
            {
                Order = productPhoto.Order,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePhoto(int id, ProductPhotoUpdateVM model)
        {
            if (!ModelState.IsValid) return View();

            var productPhoto = _context.ProductPhotos.Find(id);
            if (productPhoto is null) return NotFound();

            productPhoto.Order = model.Order;

            _context.ProductPhotos.Update(productPhoto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), "product", new { id = productPhoto.ProductId } );
        }

        [HttpGet]
        public IActionResult DeletePhoto(int id)
        {
            var productPhoto = _context.ProductPhotos.FirstOrDefault(p => p.Id == id);
            if (productPhoto is null) return NotFound();

            _fileService.Delete(productPhoto.Name);
            _context.ProductPhotos.Remove(productPhoto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Update),"product", new { id = productPhoto.ProductId });
        }
        #endregion

        [HttpGet]
        public IActionResult SetMain(int id)
        {
            var productPhoto = _context.ProductPhotos.Include(p => p.Product).FirstOrDefault(p => p.Id == id);
            if (productPhoto is null) return NotFound();

            var dbProductPhotos = _context.ProductPhotos.Include(p => p.Product)
                                                        .Where(p => p.Id != productPhoto.Id && p.ProductId == productPhoto.ProductId);

            if (!productPhoto.IsMain)
            {
                foreach (var dbProductPhoto in dbProductPhotos)
                {
                    dbProductPhoto.IsMain = false;
                    _context.ProductPhotos.Update(dbProductPhoto);
                }
                _context.SaveChanges();
            }

            productPhoto.IsMain = !productPhoto.IsMain;
            _context.ProductPhotos.Update(productPhoto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Update), "product", new { id = productPhoto.ProductId });
        }

    }
}
