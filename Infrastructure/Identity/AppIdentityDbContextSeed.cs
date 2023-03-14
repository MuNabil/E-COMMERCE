using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Any()) return;

            var user = new AppUser
            {
                DisplayName = "Nabil",
                UserName = "nabil@test.com",
                Email = "nabil@test.com",
                Address = new Address
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    State = "My State",
                    City = "My City",
                    Street = "My Street",
                    ZipCode = "2000"
                }
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }
}