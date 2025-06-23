namespace Trash_Track.Models
{
    public class Driver
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }

        public ICollection<Report> AssignedReports { get; set; }
    }

}
