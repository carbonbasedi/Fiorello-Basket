using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Duty
{
	public class DutyCreateVM
	{
        [Required]
        public string Name { get; set; }
    }
}
