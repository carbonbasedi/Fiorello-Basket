namespace Fiorello.Areas.admin.ViewModels.ProductCategory
{
	public class ProductCategoryIndexVM
	{
        public ProductCategoryIndexVM()
        {
            ProductCategories = new List<Models.ProductCategory>();
        }
        public List<Models.ProductCategory> ProductCategories { get; set; }
    }
}
