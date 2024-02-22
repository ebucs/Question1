using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Question1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Question1.Data
{
    public class Seed
    {
        public static async Task InitializeAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Run migrations
                await dbContext.Database.MigrateAsync();

                // Initialize data
                await SeedRolesAsync(serviceScope.ServiceProvider);
                await SeedUsersAndRolesAsync(serviceScope.ServiceProvider);
            }
        }

        private static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        private static async Task SeedUsersAndRolesAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<Person>>();
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            if (!dbContext.Users.Any())
            {
                // Create a new Person
                var user = new Person
                {
                    UserName = "nick@gmail.com",
                    Email = "nick@gmail.com",
                    Name = "Nick",
                    Surname = "Saint",
                    PhoneNumber = "1234567890",
                    LastLogin = DateTime.Now
                };

                // Add the Person to the database
                var result = await userManager.CreateAsync(user, "P@ssw0rd");

                if (result.Succeeded)
                {
                    // Assign user role to the person
                    await userManager.AddToRoleAsync(user, "User");

                    // Create Info for the Person
                    var info = new Info
                    {
                        PersonId = user.Id,
                        TellNo = "1234567890",
                        AddressLine1 = "123 Main St",
                        AddressLine2 = "Apt 1",
                        AddressLine3 = "none",
                        AddressCode = "ABC123",
                        PostalAddress1 = "PO Box 456",
                        PostalAddress2 = "none",
                        PostalCode = "DEF456"
                    };

                    // Add Info to the database
                    dbContext.Info.Add(info);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
