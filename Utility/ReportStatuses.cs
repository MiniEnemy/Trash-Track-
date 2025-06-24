namespace Trash_Track.Utility
{
    public static class ReportStatuses
    {
        public const string Pending = "Pending";
        public const string InProgress = "In Progress";
        public const string Resolved = "Resolved";
        public const string Cancelled = "Cancelled"; 


        public static readonly List<string> All = new()
        {
            Pending,
            InProgress,
            Resolved,
            Cancelled
        };
    }
}
