using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Models;

namespace Model
{
    public class Seed
    {
        public static async Task SeedData(TaskFlowContext context, UserManager<AppUser> userManager, ILogger<Seed> logger)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new() {UserName = "test", Email = "testuser@test.com"},
                };
                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user, "Password0");
                    if (result.Succeeded)
                    {
                        logger.LogInformation("User {UserName} created successfully", user.UserName);
                    }
                    else
                    {
                        logger.LogError("Error creating user {UserName}: {Errors}", user.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                await context.SaveChangesAsync();

                var Users = await userManager.Users.ToListAsync();
            }
        }
    }
}
