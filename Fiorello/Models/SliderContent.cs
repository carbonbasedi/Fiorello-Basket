namespace Fiorello.Models
{
	public class SliderContent
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public int SliderId { get; set; }
        public Slider Slider { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
