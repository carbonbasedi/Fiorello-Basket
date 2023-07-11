using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Duty
{
	public class DutyUpdateVM
	{
		[Required]
		public string Name { get; set; }
	}
}
