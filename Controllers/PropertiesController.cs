using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyCareApi.Data;
using PropertyCareApi.Dtos;
using PropertyCareApi.Models;

namespace PropertyCareApi.Controllers
{
    [ApiController]
    [Route("properties")]
    public class PropertiesController(PropertyCareApiDbContext db) : ControllerBase
    {
        [Authorize(Roles = "Admin, Tenant")]
        //TODO: Filter Tenant to just see their own properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetAllProperties(CancellationToken cancellationToken)
        {
            var properties = await db.Properties
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

        [Authorize(Roles = "Admin, Tenant")]
        //TODO: Filter Tenant to just see their own properties
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PropertyResponseDto>> GetPropertyById(Guid id, CancellationToken cancellationToken)
        {
            var property = await db.Properties
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

        [Authorize(Roles = "Admin, Tenant")]
        //TODO: Admins can create properties for anyone, Tenants can just create properties for themselves
        [HttpPost]
        public async Task<ActionResult<PropertyResponseDto>> CreateProperty(CreatePropertyDto dto, CancellationToken cancellationToken)
        {
            var ownerExists = await db.Users
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

            db.Properties.Add(property);
            await db.SaveChangesAsync(cancellationToken);

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