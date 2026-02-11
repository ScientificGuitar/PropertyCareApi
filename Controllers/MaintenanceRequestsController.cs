using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyCareApi.Application.Queries;
using PropertyCareApi.Data;
using PropertyCareApi.Dtos;
using PropertyCareApi.Models;

namespace PropertyCareApi.Controllers
{
    [ApiController]
    [Route("maintenance-requests")]
    public class MaintenanceRequestsController(PropertyCareApiDbContext db) : ControllerBase
    {
        [Authorize]
        // TODO: Admin retrieve all, Tenant retrieve for requests for their properties, contractor retrieve non-completed requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRequestResponseDto>>> GetAllMaintenanceRequests([FromQuery] MaintenanceRequestQueryParameters query, CancellationToken cancellationToken)
        {
            var maintenanceRequestsQuery = db.MaintenanceRequests
                .AsNoTracking()
                .AsQueryable();

            if (query.PropertyId.HasValue)
                maintenanceRequestsQuery = maintenanceRequestsQuery.Where(m => m.PropertyId == query.PropertyId.Value);
            if (query.Category.HasValue)
                maintenanceRequestsQuery = maintenanceRequestsQuery.Where(m => m.Category == query.Category.Value);
            if (query.Status.HasValue)
                maintenanceRequestsQuery = maintenanceRequestsQuery.Where(m => m.Status == query.Status.Value);
            if (query.Priority.HasValue)
                maintenanceRequestsQuery = maintenanceRequestsQuery.Where(m => m.Priority == query.Priority.Value);

            var skip = (query.PageNumber - 1) * query.PageSize;
            var totalCount = await maintenanceRequestsQuery.CountAsync(cancellationToken);
            var pagedMaintenanceRequests = await maintenanceRequestsQuery
                .OrderByDescending(m => m.CreatedAt)
                .Skip(skip)
                .Take(query.PageSize)
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

            return Ok(new
            {
                totalCount,
                pageNumber = query.PageNumber,
                pageSize = query.PageSize,
                items = pagedMaintenanceRequests
            });
        }

        [Authorize]
        // TODO: Admin retrieve all, Tenant retrieve for requests for their properties, contractor retrieve non-completed requests
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MaintenanceRequestResponseDto>> GetMaintenanceRequestById(Guid id, CancellationToken cancellationToken)
        {
            var maintenanceRequest = await db.MaintenanceRequests
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

        [Authorize(Roles = "Admin, Tenant")]
        // TODO: Property must belong to tenant
        [HttpPost]
        public async Task<ActionResult<MaintenanceRequestResponseDto>> CreateMaintenanceRequest(CreateMaintenanceRequestDto dto, CancellationToken cancellationToken)
        {
            var propertyExists = await db.Properties
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
                Status = RequestStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            db.MaintenanceRequests.Add(maintenanceRequest);
            await db.SaveChangesAsync(cancellationToken);

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

        [Authorize(Roles = "Admin, Contractor")]
        [HttpPatch("{id:guid}/approve")]
        public async Task<IActionResult> ApproveMaintenanceRequest(Guid id, CancellationToken cancellationToken)
        {
            var maintenanceRequest = await db.MaintenanceRequests.FindAsync([id], cancellationToken);
            if (maintenanceRequest is null)
                return NotFound();

            maintenanceRequest.Approve();
            await db.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [Authorize(Roles = "Admin, Contractor")]
        [HttpPatch("{id:guid}/start")]
        public async Task<IActionResult> StartMaintenanceRequest(Guid id, CancellationToken ct)
        {
            var maintenanceRequest = await db.MaintenanceRequests.FindAsync([id], ct);
            if (maintenanceRequest is null)
                return NotFound();

            maintenanceRequest.Start();
            await db.SaveChangesAsync(ct);

            return NoContent();
        }

        [Authorize(Roles = "Admin, Contractor")]
        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> CompleteMaintenanceRequest(Guid id, CancellationToken ct)
        {
            var maintenanceRequest = await db.MaintenanceRequests.FindAsync([id], ct);
            if (maintenanceRequest is null)
                return NotFound();

            maintenanceRequest.Complete();
            await db.SaveChangesAsync(ct);

            return NoContent();
        }

        [Authorize]
        // TODO: Tenant can cancel if status open and their property. Contractor can cancel if approved/assigned, Admin always
        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> CancelMaintenanceRequest(Guid id, CancellationToken ct)
        {
            var maintenanceRequest = await db.MaintenanceRequests.FindAsync([id], ct);
            if (maintenanceRequest is null)
                return NotFound();

            maintenanceRequest.Cancel();
            await db.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}