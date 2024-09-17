using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.Repositories
{
    public class TagRepository(ApplicationDbContext context) : ITagRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.Include(t => t.Author)
                                      .Include(t => t.Post)
                                      .ToListAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _context.Tags.Include(t => t.Author)
                                      .Include(t => t.Post)
                                      .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddTagAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }
        public  async Task UpdateTagAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync(); ;
        }

        public async Task DeleteTagAsync(int id)
        {

            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
        
        public bool TagExists(int id)
        {
            return _context.Tags.Any(t => t.Id == id);
        }

    }
}
