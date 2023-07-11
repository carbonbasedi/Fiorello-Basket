namespace Fiorello.Areas.admin.ViewModels.Worker
{
	public class WorkerListVM
	{
        public WorkerListVM()
        {
            Workers = new List<Models.Worker>();
        }
        public List<Models.Worker> Workers { get; set; }
    }
}
