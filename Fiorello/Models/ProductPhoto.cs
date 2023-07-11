namespace Fiorello.Models
{
	public class ProductPhoto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
