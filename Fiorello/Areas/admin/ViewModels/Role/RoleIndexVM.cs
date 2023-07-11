using Microsoft.AspNetCore.Identity;

namespace Fiorello.Areas.admin.ViewModels.Role
{
	public class RoleIndexVM
	{
        public List<IdentityRole> Roles { get; set; }
    }
}
