using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Role
{
	public class RoleCreateVM
	{
		[Required]
        public string Name { get; set; }
    }
} 
