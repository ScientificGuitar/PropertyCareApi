using System.ComponentModel.DataAnnotations;
using PropertyCareApi.Models;

namespace PropertyCareApi.Dtos
{
    public class CreateMaintenanceRequestDto
    {
        [Required]
        public Guid PropertyId { get; set; }

        [Required]
        public RequestCategory Category { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; } = null!;

        [Required]
        public PriorityLevel Priority { get; set; }
    }
}