namespace Trash_Track.Models
{
    public class ReportPickupStatus
    {
        public int Id { get; set; }

        public int ReportId { get; set; }
        public Report Report { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; }

        public bool IsPickedUp { get; set; }
        public DateTime? PickupTime { get; set; }
    }

}
