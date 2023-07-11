namespace Fiorello.Models
{
	public class ProductCategory
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
