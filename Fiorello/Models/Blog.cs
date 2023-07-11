namespace Fiorello.Models
{
	public class Blog
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string About { get; set; }
        public string Motto { get; set; }
        public string About2 { get; set; }
        public string Motto2 { get; set; }
        public string About3 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
