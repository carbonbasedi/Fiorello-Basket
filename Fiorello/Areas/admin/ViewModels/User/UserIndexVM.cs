namespace Fiorello.Areas.admin.ViewModels.User
{
	public class UserIndexVM
	{
        public UserIndexVM()
        {
            Users = new List<UserVM>();
        }
        public  List<UserVM> Users { get; set; }
    }
}
