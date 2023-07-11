namespace Fiorello.Models
{
	public class Duty
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Worker> Workers { get; set; }
    }
}
