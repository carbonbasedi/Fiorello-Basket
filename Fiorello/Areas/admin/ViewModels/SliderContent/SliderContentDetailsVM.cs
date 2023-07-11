namespace Fiorello.Areas.admin.ViewModels.SliderContent
{
	public class SliderContentDetailsVM
	{
		public string Title { get; set; }
		public string About { get; set; }
		public Models.Slider Slider { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
	}
}
