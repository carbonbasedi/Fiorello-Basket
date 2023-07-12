namespace Fiorello.ViewModels.Basket
{
    public class BasketModel
    {
        public int Id { get; set; }
        public string Title { get; set; }   
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string PhotoName { get; set; }
    }
}
