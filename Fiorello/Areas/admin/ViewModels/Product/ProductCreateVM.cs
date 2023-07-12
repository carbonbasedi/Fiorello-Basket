using Fiorello.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Product
{
	public class ProductCreateVM
	{
        public ProductCreateVM()
        {
			Photos = new List<IFormFile>();
        }

        [Required]
		[MinLength(3)]
		[MaxLength(20)]
		public string Title { get; set; }

		[Required]
        public decimal Price { get; set; }

        [Required]
		[MinLength(10)]
        public string About { get; set; }

        [Required]
        public int Stock { get; set; }

        public ProductType Type { get; set; }

        [Required]
		[MinLength(3)]
		public string Description { get; set; }

		[Required]
		[Display(Name = "Additional Info")]
		public string AdditionalInfo { get; set; }

        public List<IFormFile> Photos { get; set; }

        [Display(Name = "Product Category")]
		public int ProductCategoryId { get; set; }
		public List<SelectListItem>? ProductCategories { get; set; }
	}
}
