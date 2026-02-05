using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCareApi.Dtos
{
    public class PropertyResponseDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = null!;
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}