using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Duty
{
	public class DutyDetailsVM
	{
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public List<Models.Worker> Workers { get; set; }
	}
}