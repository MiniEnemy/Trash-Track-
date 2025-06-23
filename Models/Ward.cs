namespace Trash_Track.Models
{
    public class Ward
    {
        public int Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }

        public PickupSchedule PickupSchedule { get; set; }
        public ICollection<Report> Reports { get; set; }
    }

}
