using Fiorello.Models;

namespace Fiorello.ViewModels.FaqPage
{
	public class FaqPageIndexVM
	{
        public FaqPageIndexVM()
        {
            Faqs = new List<Faq>();
        }
        public List<Faq> Faqs { get; set; }
    }
}
