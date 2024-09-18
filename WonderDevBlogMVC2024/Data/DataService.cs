using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Enums;

namespace WonderDevBlogMVC2024.Data
{
    public class DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        readonly ApplicationDbContext _context = dbContext;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task SetupDB()
        {
            //Run the migration async
            await _context.Database.MigrateAsync();

            //Add roles into system;
            await SeedRolesAsync();
            //add users into system;
            await SeedUsersAsync();
        }
        //Assign users to roles
        private async Task SeedUsersAsync()
        {
            if (_context.Users.Any())
            {
                return;
            }

            ApplicationUser adminUser = new()
            {
                UserName = "Shelly.Wonder@Outlook.com",
                Email = "Shelly.Wonder@Outlook.com",
                FirstName = "Shelly",
                LastName = "Wonder",
                PhoneNumber = "4025555555",
                EmailConfirmed = true
            };
            ApplicationUser modUser = new()
            {
                UserName = "Shelly.Wonder@Outlook.com",
                Email = "Shelly.Wonder@Outlook.com",
                FirstName = "Shelly",
                LastName = "Wonder",
                PhoneNumber = "4025555555",
                EmailConfirmed = true
            };
            ApplicationUser author = new()
            {
                UserName = "Shelly.Wonder@Outlook.com",
                Email = "Shelly.Wonder@Outlook.com",
                FirstName = "Shelly",
                LastName = "Wonder",
                PhoneNumber = "4025555555",
                EmailConfirmed = true
            };
            ApplicationUser commentor = new()
            {
                UserName = "Shelly.Wonder@Outlook.com",
                Email = "Shelly.Wonder@Outlook.com",
                FirstName = "Shelly",
                LastName = "Wonder",
                PhoneNumber = "4025555555",
                EmailConfirmed = true
            };
            try
            {
                if (await _userManager.FindByEmailAsync(adminUser.Email) is null)
                {
                    await _userManager.CreateAsync(adminUser, "Abc&123!");
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
                if (await _userManager.FindByEmailAsync(modUser.Email) is null)
                {
                    await _userManager.CreateAsync(modUser, "Abc&123!");
                    await _userManager.AddToRoleAsync(modUser, "Moderator");

                }
                if (await _userManager.FindByEmailAsync(author.Email) is null)
                {
                    await _userManager.CreateAsync(author, "Abc&123!");
                    await _userManager.AddToRoleAsync(author, "Author");

                }
                if (await _userManager.FindByEmailAsync(commentor.Email) is null)
                {
                    await _userManager.CreateAsync(commentor, "Abc&123!");
                    await _userManager.AddToRoleAsync(commentor, "Commentor");

                }

            }
            catch (Exception ex)
            {


            }
        }
        private async Task SeedRolesAsync()
        {
            if (_roleManager.Roles.Any())
            {
                return;
            }

            foreach (string role in Enum.GetNames(typeof(BlogRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

        }
    }
}

