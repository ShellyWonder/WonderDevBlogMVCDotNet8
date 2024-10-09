using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.SeedData
{
    public class BlogsDataService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogsDataService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task InitializeAsync()
        {
           
            // Check if the database has been seeded
            if (_context.Blogs.Any())
            {
                return;   // DB has been seeded
            }

            // Seed User
            
            var FirstName = "Shelly";
            var LastName = "Wonder";
            var authorEmail = "Shelly.Wonder@Outlook.com";
            var authorId = "f0c37325-5c21-47a7-a5d9-fc6b03cbd187";  // Predefined AuthorId

            var author = await _userManager.FindByEmailAsync(authorEmail);
            if (author == null)
            {
                // Create the user if not found
                author = new ApplicationUser
                {
                    Id = authorId,
                    FirstName = FirstName,
                    LastName = FirstName,
                    UserName = authorEmail,
                    Email = authorEmail,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(author, "Abc&123!"); 
            }

            // Seed Blogs
            var blogs = new List<Blog>
            {
                new()
                {
                    Name = "CSS Tips and Tricks",
                    Description = "A blog dedicated to sharing practical CSS tips and tricks for web developers.",
                    Created = DateTime.UtcNow,
                    AuthorId = authorId // Assigning AuthorId to Shelly.Wonder
                },
                new()
                {
                    Name = "Best Front Development Frameworks for .NET developers",
                    Description = "Exploring the best front-end frameworks for .NET developers and how to integrate them into projects.",
                    Created = DateTime.UtcNow,
                    AuthorId = authorId
                },
                new() 
                {
                    Name = "How to use the 12-week year system to prepare for the Web Development Job Market",
                    Description = "A guide on using the 12-week year system to efficiently prepare for the web development job market.",
                    Created = DateTime.UtcNow,
                    AuthorId = authorId
                }
            };

            _context.Blogs.AddRange(blogs);
            await _context.SaveChangesAsync();
        }
    }
}
