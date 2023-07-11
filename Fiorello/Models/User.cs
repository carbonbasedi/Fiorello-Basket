using Microsoft.AspNetCore.Identity;

namespace Fiorello.Models
{
	public class User : IdentityUser
	{
		public string FullName { get; set; }
		public string Country { get; set; }
	}
}
