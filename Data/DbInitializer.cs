using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PropertyCareApi.Models;

namespace PropertyCareApi.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(PropertyCareApiDbContext context)
        {
            if (context.Users.Any())
                return;

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@test.com",
                Role = UserRole.Admin,
                PasswordHash = HashPassword("password")
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
                Priority = PriorityLevel.High
            };

            context.MaintenanceRequests.Add(request);

            await context.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            // Quick and easy hash for now
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}