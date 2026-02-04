using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCareApi.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }

        public string EntityType { get; set; } = null!;
        public Guid EntityId { get; set; }
        public string Action { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}