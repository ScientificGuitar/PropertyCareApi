using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyCareApi.Data;
using PropertyCareApi.Dtos;
using PropertyCareApi.Models;

namespace PropertyCareApi.Controllers
{
    [ApiController]
    [Route("properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyCareApiDbContext _db;

        public PropertiesController(PropertyCareApiDbContext db)
            => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetAllProperties(CancellationToken cancellationToken)
        {
            var properties = await _db.Properties
                .AsNoTracking()
                .Select(p => new PropertyResponseDto
                {
                    Id = p.Id,
                    Address = p.Address,
                    OwnerId = p.OwnerId,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Ok(properties);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PropertyResponseDto>> GetPropertyById(Guid id, CancellationToken cancellationToken)
        {
            var property = await _db.Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (property == null)
                return NotFound();

            return new PropertyResponseDto
            {
                Id = property.Id,
                Address = property.Address,
                OwnerId = property.OwnerId,
                CreatedAt = property.CreatedAt
            };
        }

        [HttpPost]
        public async Task<ActionResult<PropertyResponseDto>> CreateProperty(CreatePropertyDto dto, CancellationToken cancellationToken)
        {
            var ownerExists = await _db.Users
                .AnyAsync(u => u.Id == dto.OwnerId, cancellationToken);

            if (!ownerExists)
                return NotFound(new ProblemDetails
                {
                    Title = "Owner not found",
                    Detail = $"Owner with id '{dto.OwnerId}' does not exist",
                    Status = StatusCodes.Status404NotFound
                });

            var property = new Property
            {
                Id = Guid.NewGuid(),
                Address = dto.Address,
                OwnerId = dto.OwnerId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Properties.Add(property);
            await _db.SaveChangesAsync(cancellationToken);

            var response = new PropertyResponseDto
            {
                Id = property.Id,
                Address = property.Address,
                OwnerId = property.OwnerId,
                CreatedAt = property.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetPropertyById),
                new { id = property.Id },
                response
            );
        }
    }
}