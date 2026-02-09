namespace PropertyCareApi.Models
{
    public class MaintenanceRequest : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public RequestCategory Category { get; set; }
        public string Description { get; set; } = null!;
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Submitted;

        public DateTime? CompletedAt { get; set; }
    }

    public enum RequestPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum RequestStatus
    {
        Submitted,
        Approved,
        InProgress,
        Completed,
        Cancelled
    }

    public enum RequestCategory
    {
        Plumbing,
        Electrical,
        Carpentry,
        Cleaning,
        Painting,
        Roofing,
        Other
    }
}