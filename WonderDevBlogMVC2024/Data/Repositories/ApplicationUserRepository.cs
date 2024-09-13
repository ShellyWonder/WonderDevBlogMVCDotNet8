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
    }
}
