
using PropertyCare.Domain.Entities;

namespace PropertyCare.Infrastructure;

public class DbInitializer
{
    public static async Task SeedAsync(PropertyCareDbContext context)
    {
        if (context.Users.Any())
            return;

        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@test.com",
            Role = UserRole.Admin,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")
        };

        context.Users.Add(adminUser);

        var property = new Property
        {
            Id = Guid.NewGuid(),
            OwnerId = adminUser.Id,
            Address = "123 Seed Street"
        };

        context.Properties.Add(property);

        var request = new MaintenanceRequest
        {
            Id = Guid.NewGuid(),
            PropertyId = property.Id,
            Category = RequestCategory.Plumbing,
            Description = "Leaking pipe under sink",
            Priority = RequestPriority.High
        };

        context.MaintenanceRequests.Add(request);

        await context.SaveChangesAsync();
    }
}
