using Fiorello.Enums;

namespace Fiorello.Models
{
	public class Product
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string About { get; set; }
        public ProductType Type { get; set; }
        public string Description { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<ProductPhoto> ProductPhotos { get; set; }

    }
}
