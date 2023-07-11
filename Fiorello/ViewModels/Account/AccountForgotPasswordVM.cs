using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels.Account
{
    public class AccountForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
