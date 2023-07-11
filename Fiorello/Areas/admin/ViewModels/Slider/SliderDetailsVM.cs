namespace Fiorello.Areas.admin.ViewModels.Slider
{
	public class SliderDetailsVM
	{
		public string Title { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public List<Models.SliderContent> SliderContents { get; set; }
	}
}
