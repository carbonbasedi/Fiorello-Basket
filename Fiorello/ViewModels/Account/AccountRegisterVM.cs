using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels.Account
{
	public class AccountRegisterVM
	{
		[Required(ErrorMessage = "Enter Fullname")]
		[MaxLength(50, ErrorMessage = "Fullname can't be longer than 50 characters")]
		public string Fullname { get; set; }

        [Required(ErrorMessage ="Enter Username")]
        [MaxLength(50, ErrorMessage ="Username can't be longer than 50 characters")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage ="Wrong email format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Country { get; set; }

        [Required(ErrorMessage ="Phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
    }
}
