namespace Trash_Track.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public int WardNumber { get; set; }
        public string Role { get; set; }
        public bool IsLocked { get; set; }
        public List<string>? AvailableRoles { get; set; }  // For dropdown
        public string? SelectedRole { get; set; }          // For form post

    }

}
