namespace Trash_Track.Models
{
    public class Report
    {
        public int Id { get; set; }

        public string ReporterName { get; set; }
        public string? ReporterUserId { get; set; }


        public int WardId { get; set; }
        public string Description { get; set; }

        public string? PhotoPath { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Resolved, Ignored

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? AssignedDriverId { get; set; }

        public Ward Ward { get; set; }
        public Driver? AssignedDriver { get; set; }
        public string? Remarks { get; set; } 

    }

}
