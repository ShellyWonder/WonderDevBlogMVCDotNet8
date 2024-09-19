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

        public  async Task<ApplicationUser?>UpdateUserAsync(ApplicationUser updatedUser)
        {
            var existingUser = await _context.Users
                                         .FindAsync(updatedUser.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {updatedUser.Id} was not found.");
            }
            existingUser.UserName = updatedUser.UserName;
            existingUser.Email = updatedUser.Email;
            existingUser.ImageData = updatedUser.ImageData;
            existingUser.ImageType = updatedUser.ImageType;
            // Update other properties as needed

            // Save changes to the database
            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
    
}
