namespace Trash_Track.Models
{
    public class PickupScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public int WardNumber { get; set; }
        public TimeSpan PickupTime { get; set; }
        public bool IsCompleted { get; set; }
    }

}