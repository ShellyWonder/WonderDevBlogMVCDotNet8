using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> CommentExistsAsync(int id)
        {
            return await _context.Comments.AnyAsync(e => e.Id == id);
        }

        public  async Task CreateCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments
            .Include(c => c.Author)
            .Include(c => c.Moderator)
            .Include(c => c.Post)
            .ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                                 .Include (c => c.Author)
                                 .Include(c => c.Moderator)
                                 .Include(c => c.Post)
                                 .FirstOrDefaultAsync(c=> c.Id == id);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
