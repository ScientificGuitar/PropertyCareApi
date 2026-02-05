namespace PropertyCareApi.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }

        // Navigation
        public ICollection<Property> Properties { get; set; } = [];
    }

    public enum UserRole
    {
        Tenant,
        Contractor,
        Admin
    }
}

