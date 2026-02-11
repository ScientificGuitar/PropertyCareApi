namespace PropertyCareApi.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }

        public User() { }
        public User(string email, string passwordHash, UserRole role)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

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

