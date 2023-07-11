using Fiorello.Models;

namespace Fiorello.Areas.admin.ViewModels.Faq
{
	public class FaqListVM
	{
        public FaqListVM()
        {
            Faqs = new List<Models.Faq>();
        }
        public List<Models.Faq> Faqs { get; set; }
    }
}
