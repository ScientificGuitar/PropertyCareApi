namespace PropertyCareApi.Models
{
    public class MaintenanceRequest : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public RequestCategory Category { get; set; }
        public string Description { get; set; } = null!;
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Open;

        public DateTime? CompletedAt { get; set; }

        public void Approve()
        {
            if (Status != RequestStatus.Open)
                throw new InvalidOperationException("Only open requests can be approved.");
            Status = RequestStatus.Approved;
        }

        public void Start()
        {
            if (Status != RequestStatus.Approved)
                throw new InvalidOperationException("Only approved requests can be started.");
            Status = RequestStatus.InProgress;
        }

        public void Complete()
        {
            if (Status != RequestStatus.InProgress)
                throw new InvalidOperationException("Only in-progress requests can be completed.");
            Status = RequestStatus.Completed;
        }

        public void Cancel()
        {
            if (Status is RequestStatus.Completed or RequestStatus.Cancelled)
                throw new InvalidOperationException("Completed or cancelled requests cannot be cancelled.");
            Status = RequestStatus.Cancelled;
        }
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
        Open,
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