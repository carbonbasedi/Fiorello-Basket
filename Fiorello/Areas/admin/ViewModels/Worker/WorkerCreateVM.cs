using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.admin.ViewModels.Worker
{
    public class WorkerCreateVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public int DutyId { get; set; }
        public List<SelectListItem>? DutyTitles { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
