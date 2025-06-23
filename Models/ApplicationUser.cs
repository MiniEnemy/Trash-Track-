using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Trash_Track.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string ContactNumber { get; set; }

        [Range(1, 32, ErrorMessage = "Ward number must be between 1 and 32.")]
        public int WardNumber { get; set; }
    }
}
