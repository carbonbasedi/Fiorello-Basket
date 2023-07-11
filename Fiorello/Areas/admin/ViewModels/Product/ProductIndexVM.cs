namespace Fiorello.Areas.admin.ViewModels.Product
{
	public class ProductIndexVM
	{
        public ProductIndexVM()
        {
            Products = new List<Models.Product>();
        }
        public List<Models.Product> Products { get; set; }
    }
}
