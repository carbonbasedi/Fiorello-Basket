namespace Fiorello.Areas.admin.ViewModels.Blog
{
	public class BlogListVM
	{
        public BlogListVM()
        {
            Blogs = new List<Models.Blog>();
        }
        public List<Models.Blog> Blogs { get; set; }
    }
}
