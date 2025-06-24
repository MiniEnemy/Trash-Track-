namespace Trash_Track.Models
{
    namespace Trash_Track.Models
    {
        public class DriverPickupStatus
        {
            public int Id { get; set; }

            public int DriverId { get; set; }
            public Driver Driver { get; set; }

            public int ScheduleId { get; set; }
            public PickupSchedule Schedule { get; set; }

            public DateTime CompletedOn { get; set; }
        }
    }

}