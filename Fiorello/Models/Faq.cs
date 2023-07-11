namespace Fiorello.Models
{
	public class Faq
	{
        public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
