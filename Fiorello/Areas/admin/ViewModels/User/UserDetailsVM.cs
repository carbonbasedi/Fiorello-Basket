namespace Fiorello.Areas.admin.ViewModels.User
{
	public class UserDetailsVM
	{
        public string Username { get; set; }
        public string Fullname { get; set; }
		public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
