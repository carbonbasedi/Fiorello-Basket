using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels.Account
{
	public class AccountLoginVM
	{
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
