using Fiorello.Enums;
using Fiorello.Models;

namespace Fiorello.Areas.admin.ViewModels.Product
{
    public class ProductDetailsVM
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
        public List<ProductPhoto> Photos { get; set; }
        public Models.ProductCategory ProductCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
