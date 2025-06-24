using Microsoft.AspNetCore.Identity;

namespace Trash_Track.Models
{
    public class Driver
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }
        public ICollection<PickupSchedule> AssignedPickupSchedules { get; set; }
        public ICollection<Report> AssignedReports { get; set; }
        public string Status { get; set; } //pickedup or not
        public string? UserId { get; set; }  // FK to AspNetUsers.Id
        public virtual IdentityUser User { get; set; }

    }

}
