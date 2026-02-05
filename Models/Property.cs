namespace PropertyCareApi.Models
{
    public class Property : BaseEntity
    {
        public string Address { get; set; } = null!;

        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        // Navigation
        public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = [];
    }
}