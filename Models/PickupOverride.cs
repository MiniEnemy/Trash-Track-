using System.ComponentModel.DataAnnotations;

namespace Trash_Track.Models
{
    public class PickupOverride
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ward")]
        public int WardId { get; set; }  // THIS is the bound field from form

        public Ward Ward { get; set; }
        public DateTime StartDate { get; set; }   // e.g., 2025-06-20
        public DateTime EndDate { get; set; }     // e.g., 2025-07-04

        public TimeSpan? NewTime { get; set; }    // null if cancelled
        public bool IsCancelled { get; set; } = false;
        public string? Message { get; set; }

    }

}
