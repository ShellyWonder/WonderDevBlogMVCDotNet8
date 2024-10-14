using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using WonderDevBlogMVC2024.Enums;

namespace WonderDevBlogMVC2024.Data.SeedData
{
    public class RolesDataService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RolesDataService> _logger ;

        public RolesDataService(ApplicationDbContext context, 
                                RoleManager<IdentityRole> roleManager, 
                                UserManager<ApplicationUser> userManager, 
                                ILogger<RolesDataService> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task SetupDBAsync()
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
                GitHubUrl = "https://github.com/ShellyWonder",
                LinkdInUrl = "https://linkedIn.com/ShellyWonder",
                EmailConfirmed = true
            };
            ApplicationUser modUser = new()
            {
                UserName = "Shelly.Wonder@Outlook.com",
                Email = "Shelly.Wonder@Outlook.com",
                FirstName = "Shelly",
                LastName = "Wonder",
                PhoneNumber = "4025555555",
                GitHubUrl = "https://github.com/ShellyWonder",
                LinkdInUrl = "https://linkedIn.com/ShellyWonder",
                EmailConfirmed = true
            };
            ApplicationUser author = new()
            {
                UserName = "Shelly.Wonder@Outlook.com",
                Email = "Shelly.Wonder@Outlook.com",
                FirstName = "Shelly",
                LastName = "Wonder",
                PhoneNumber = "4025555555",
                GitHubUrl = "https://github.com/ShellyWonder",
                LinkdInUrl = "https://linkedIn.com/ShellyWonder",
                EmailConfirmed = true
            };
            ApplicationUser Commentator = new()
            {
                UserName = "Shelly.Wonder@Outlook.com",
                Email = "Shelly.Wonder@Outlook.com",
                FirstName = "Shelly",
                LastName = "Wonder",
                PhoneNumber = "4025555555",
                GitHubUrl = "https://github.com/ShellyWonder",
                LinkdInUrl = "https://linkedIn.com/ShellyWonder",
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
                if (await _userManager.FindByEmailAsync(Commentator.Email) is null)
                {
                    await _userManager.CreateAsync(Commentator, "Abc&123!");
                    await _userManager.AddToRoleAsync(Commentator, "Commentator");

                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while seeding users.");
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

