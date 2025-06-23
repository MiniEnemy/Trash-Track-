namespace Trash_Track.Models
{
    public class ReportViewModel
    {
        public int Id { get; set; }

        public string ReporterName { get; set; }

        public int WardId { get; set; }
        public string Description { get; set; }

        public string? PhotoPath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Ward Ward { get; set; }
    }
}
