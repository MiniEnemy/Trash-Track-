using System.ComponentModel.DataAnnotations;
using Trash_Track.Admin;

namespace Trash_Track.Models
{
    public class PickupOverride
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ward")]
        public int WardId { get; set; }

        public Ward Ward { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [CompareDate(nameof(StartDate), ErrorMessage = "End date must be after start date.")]
        public DateTime EndDate { get; set; }

        [Display(Name = "New Pickup Time")]
        public TimeSpan? NewTime { get; set; }

        [Display(Name = "Cancel Pickup")]
        public bool IsCancelled { get; set; }

        [StringLength(300)]
        public string? Message { get; set; }
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }
    }

}


