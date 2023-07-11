using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Blog
{
	public class BlogUpdateVM
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Subtitle { get; set; }

		[Required]
		public string About { get; set; }

		[Required]
		[MaxLength(100)]
		public string Motto { get; set; }

		[Required]
		public string About2 { get; set; }

		[Required]
		public string Motto2 { get; set; }

		[Required]
		public string About3 { get; set; }

		public IFormFile? Photo { get; set; }
		public string? ImageUrl { get; set; }
	}
}
