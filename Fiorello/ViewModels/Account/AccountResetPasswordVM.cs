using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels.Account
{
    public class AccountResetPasswordVM
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
