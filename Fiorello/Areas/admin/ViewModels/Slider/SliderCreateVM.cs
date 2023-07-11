using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Slider
{
	public class SliderCreateVM
	{
		[Required(ErrorMessage = "Title is required")]
		[MaxLength(20, ErrorMessage = "Title mustn't exceed 20 characters")]
		public string Title { get; set; }
	}
}
