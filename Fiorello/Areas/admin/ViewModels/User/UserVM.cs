using Microsoft.AspNetCore.Identity;

namespace Fiorello.Areas.admin.ViewModels.User
{
    public class UserVM
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
