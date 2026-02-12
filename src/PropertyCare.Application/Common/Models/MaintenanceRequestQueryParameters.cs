using PropertyCare.Domain.Entities;

namespace PropertyCare.Application.Common.Models;

public class MaintenanceRequestQueryParameters
{
    public const int MaxPageSize = 100;

    private int _pageSize = 20;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(MaxPageSize, value);
    }

    public Guid? PropertyId { get; set; }
    public RequestCategory? Category { get; set; }
    public RequestStatus? Status { get; set; }
    public RequestPriority? Priority { get; set; }
}
