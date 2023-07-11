using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.SliderContent
{
	public class SliderContentCreateVM
	{
		[Required]
		[MinLength(3)]
		[MaxLength(15)]
		public string Title { get; set; }

		[Required]
		[MinLength(5)]
		[MaxLength(100)]
		public string About { get; set; }

		[Display(Name = "Slider Title")]
		public int SliderId { get; set; }
		public List<SelectListItem>? SliderTitleList { get; set; }

    }
}
