using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyCareApi.Models;

namespace PropertyCareApi.Dtos
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}