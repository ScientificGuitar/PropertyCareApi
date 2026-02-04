using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCareApi.Models
{
    public class MaintenanceRequest
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public PriorityLevel Priority { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Submitted;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }

    public enum PriorityLevel
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
        Scheduled,
        InProgress,
        Completed,
        Cancelled
    }
}