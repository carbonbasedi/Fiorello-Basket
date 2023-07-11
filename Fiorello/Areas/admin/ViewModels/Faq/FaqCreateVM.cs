using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Faq
{
	public class FaqCreateVM
	{
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
