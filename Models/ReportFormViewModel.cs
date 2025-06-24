namespace Trash_Track.Models
{
    public class ReportFormViewModel
    {
        public string ReporterName { get; set; }

        public int WardId { get; set; }
        public string Description { get; set; }

        public IFormFile? Photo { get; set; } 
    }
}
