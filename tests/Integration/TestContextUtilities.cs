using Dive.App.Data;
using Dive.App.Models;

namespace Dive.Tests.Integration
{
    public class TestContextUtilities
    {
        public static void InitializeDatabase(DiveContext context)
        {
            context.Users.Add(new User
            {
                UserName = "john",
                NormalizedUserName = "JOHN",
                Email = "john@example.com",
                NormalizedEmail = "JOHN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEJEKlvDv3VK19UUeevy6kdBOia1kmFOsBwMjClbnwihchVw41y4pDm476u9XF0fbwA==", // Test1234!
                SecurityStamp = "RYYSMWSWMFRL3V2BZF4OIZC5TGMJAIIN",
                ConcurrencyStamp = "ec1b9a72-754e-4af0-9166-4ae890751a22",
                FirstName = "John",
                LastName = "Doe"
            });
            context.Users.Add(new User
            {
                UserName = "jane",
                NormalizedUserName = "JANE",
                Email = "jane@example.com",
                NormalizedEmail = "JANE@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEJEKlvDv3VK19UUeevy6kdBOia1kmFOsBwMjClbnwihchVw41y4pDm476u9XF0fbwA==", // Test1234!
                SecurityStamp = "RYYSMWSWMFRL3V2BZF4OIZC5TGMJAIIN",
                ConcurrencyStamp = "ec1b9a72-754e-4af0-9166-4ae890751a22",
                FirstName = "Jane",
                LastName = "Doe"
            });

            context.SaveChanges();
        }
    }
}