using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;

namespace WonderDevBlogMVC2024.Data.Repositories
{
    public class ApplicationUserRepository(ApplicationDbContext context) : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        /// <summary>
        /// ApplicationUser inherits id from IdentityUser whose Id is a guid.
        /// Therefore, GetUserByIdAsync has a string id parameter)
        /// </summary>
       
        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            // Await the result and store it in a variable
            var user = await _context.Users
                                     .Where(x => x.Id == id)
                                     .SingleOrDefaultAsync();

            // Check if the user is null and throw an exception if needed
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }

            return user;
        }

            public async Task<IEnumerable<string?>> GetAllAuthorsAsync()
            {
            var blogAuthors = _context.Blogs
                .Where(b => b.Author != null)
                .Select(b => b.Author!.FullName)
                .Distinct();

            var postAuthors = _context.Posts
                .Where(p => p.Author != null)
                .Select(p => p.Author!.FullName)
                .Distinct();

            var allAuthors = blogAuthors
                .Union(postAuthors)
                .Distinct();

            return await allAuthors.ToListAsync();
            }

    }


}
    
