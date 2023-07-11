namespace Fiorello.Areas.admin.ViewModels.Worker
{
	public class WorkerDetailsVM
	{
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Models.Duty Duty { get; set; }
    }
}
