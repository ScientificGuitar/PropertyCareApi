using System;
using System.Collections.Generic;

namespace PropertyCareApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }

    public enum UserRole
    {
        Tenant,
        Contractor,
        Admin
    }
}

