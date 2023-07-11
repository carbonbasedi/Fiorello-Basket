namespace Fiorello.Areas.admin.ViewModels.SliderContent
{
	public class SliderContentListVM
	{
        public SliderContentListVM()
        {
            SliderContents = new List<Models.SliderContent>();
        }
        public List<Models.SliderContent> SliderContents { get; set; }
    }
}
