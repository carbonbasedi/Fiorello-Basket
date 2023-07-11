using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.ProductCategory
{
	public class ProductCategoryCreateVM
	{
		[Required(ErrorMessage = "Name of new Category is required")]
		[MinLength(3, ErrorMessage = "Name must consist of at least 3 characters")]
		[MaxLength(20, ErrorMessage = "Name can not be set over 20 characters")]
		[Display(Name = "Title")]
		public string Name { get; set; }
	}
}
