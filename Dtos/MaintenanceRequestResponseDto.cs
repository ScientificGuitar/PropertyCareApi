using PropertyCareApi.Models;

namespace PropertyCareApi.Dtos
{
    public class MaintenanceRequestResponseDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public RequestCategory Category { get; set; }
        public string Description { get; set; } = null!;
        public PriorityLevel Priority { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}