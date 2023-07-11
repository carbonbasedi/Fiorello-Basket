namespace Fiorello.Areas.admin.ViewModels.ProductCategory
{
	public class ProductCategoryDetailsVM
	{
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public List<Models.Product> Products { get; set; }
	}
}
