namespace Fiorello.ViewModels.Blogs
{
	public class BlogsIndexVM
	{
        public BlogsIndexVM()
        {
            Blogs = new List<Models.Blog>();
        }
        public List<Models.Blog> Blogs { get; set; }
    }
}
