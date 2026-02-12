namespace PropertyCare.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public string EntityType { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string Action { get; set; } = null!;
}
