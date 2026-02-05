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
    [Route("maintenance-requests")]
    public class MaintenanceRequestsController : ControllerBase
    {
        private readonly PropertyCareApiDbContext _db;

        public MaintenanceRequestsController(PropertyCareApiDbContext db)
            => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRequestResponseDto>>> GetAllMaintenanceRequests(CancellationToken cancellationToken)
        {
            var maintenanceRequests = await _db.MaintenanceRequests
                .AsNoTracking()
                .Select(m => new MaintenanceRequestResponseDto
                {
                    Id = m.Id,
                    PropertyId = m.PropertyId,
                    Category = m.Category,
                    Description = m.Description,
                    Priority = m.Priority,
                    Status = m.Status,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Ok(maintenanceRequests);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MaintenanceRequestResponseDto>> GetMaintenanceRequestById(Guid id, CancellationToken cancellationToken)
        {
            var maintenanceRequest = await _db.MaintenanceRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (maintenanceRequest == null)
                return NotFound();

            return new MaintenanceRequestResponseDto
            {
                Id = maintenanceRequest.Id,
                PropertyId = maintenanceRequest.PropertyId,
                Category = maintenanceRequest.Category,
                Description = maintenanceRequest.Description,
                Priority = maintenanceRequest.Priority,
                Status = maintenanceRequest.Status,
                CreatedAt = maintenanceRequest.CreatedAt
            };
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRequestResponseDto>> Create(CreateMaintenanceRequestDto dto, CancellationToken cancellationToken)
        {
            var propertyExists = await _db.Properties
                .AnyAsync(p => p.Id == dto.PropertyId, cancellationToken);

            if (!propertyExists)
                return NotFound(new ProblemDetails
                {
                    Title = "Property not found",
                    Detail = $"Property with id '{dto.PropertyId}' does not exist",
                    Status = StatusCodes.Status404NotFound
                });

            var maintenanceRequest = new MaintenanceRequest
            {
                Id = Guid.NewGuid(),
                PropertyId = dto.PropertyId,
                Category = dto.Category,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = RequestStatus.Submitted,
                CreatedAt = DateTime.UtcNow
            };

            _db.MaintenanceRequests.Add(maintenanceRequest);
            await _db.SaveChangesAsync(cancellationToken);

            var response = new MaintenanceRequestResponseDto
            {
                Id = maintenanceRequest.Id,
                PropertyId = maintenanceRequest.PropertyId,
                Category = maintenanceRequest.Category,
                Description = maintenanceRequest.Description,
                Priority = maintenanceRequest.Priority,
                Status = maintenanceRequest.Status,
                CreatedAt = maintenanceRequest.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetMaintenanceRequestById),
                new { id = maintenanceRequest.Id },
                response
            );
        }


    }
}