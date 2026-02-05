using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PropertyCareApi.Models;

namespace PropertyCareApi.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        [MaxLength(256)]
        public string Password { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }
    }
}