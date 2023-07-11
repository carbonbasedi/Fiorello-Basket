namespace Fiorello.Models
{
	public class Worker
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsDeleted { get; set; }
        public int DutyId { get; set; }
        public Duty Duty { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
