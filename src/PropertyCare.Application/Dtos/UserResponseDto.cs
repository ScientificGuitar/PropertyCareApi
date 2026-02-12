using PropertyCare.Domain.Entities;

namespace PropertyCare.Application.Dtos;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
