using System.ComponentModel.DataAnnotations;

namespace PropertyCare.Application.Dtos;

public class CreatePropertyDto
{
    [Required]
    public Guid OwnerId { get; set; }

    [Required]
    [MaxLength(256)]
    public string Address { get; set; } = null!;
}
