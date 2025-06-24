namespace Trash_Track.Models
{
    public class PickupSchedule
    {
        public int Id { get; set; }
        public int WardId { get; set; }
        public DayOfWeek PickupDay { get; set; }
        public TimeSpan PickupTime { get; set; }
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }
        public Ward Ward { get; set; }
    }

}
