namespace Trash_Track.Models.ViewModels
{
    public class PickupViewModel
    {
        public int WardNumber { get; set; }
        public string WardName { get; set; }

        public DayOfWeek? RegularDay { get; set; }
        public TimeSpan? RegularTime { get; set; }
        public string? RegularDriver { get; set; }
        public DayOfWeek? OverrideDay { get; set; } 


        public bool IsOverrideToday { get; set; }
        public bool IsCancelled { get; set; }
        public TimeSpan? OverrideTime { get; set; }
        public string? OverrideMessage { get; set; }
        public string? OverrideDriver { get; set; }
    }

}
