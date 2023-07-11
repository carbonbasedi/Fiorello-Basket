namespace Fiorello.ViewModels.Products
{
	public class ProductsIndexVM
	{
        public ProductsIndexVM()
        {
            Products = new List<Models.Product>();
        }
        public List<Models.Product> Products { get; set; }
    }
}
