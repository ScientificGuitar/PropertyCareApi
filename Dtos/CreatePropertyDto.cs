using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCareApi.Dtos
{
    public class CreatePropertyDto
    {
        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Address { get; set; } = null!;
    }
}