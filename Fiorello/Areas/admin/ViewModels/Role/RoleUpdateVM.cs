using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Role
{
	public class RoleUpdateVM
	{
		[Required]
		public string Name { get; set; }
	}
}
