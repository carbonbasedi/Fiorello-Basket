namespace Fiorello.Areas.admin.ViewModels.Duty
{
	public class DutyIndexVM
	{
        public DutyIndexVM()
        {
            Duties = new List<Models.Duty>();
        }
        public List<Models.Duty> Duties { get; set; }
    }
}
