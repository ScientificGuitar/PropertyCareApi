using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PropertyCareApi.Models;

namespace PropertyCareApi.Dtos
{
    public class CreateMaintenanceRequestDto
    {
        [Required]
        public Guid PropertyId { get; set; }

        [Required]
        [MaxLength(128)]
        public string Category { get; set; } = null!;

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; } = null!;

        [Required]
        public PriorityLevel Priority { get; set; }
    }
}