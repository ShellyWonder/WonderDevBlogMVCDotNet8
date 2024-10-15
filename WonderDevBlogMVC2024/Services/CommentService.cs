using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _commentRepository.GetAllCommentsAsync();
        }
        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _commentRepository.GetCommentByIdAsync(id);
        }
        public  async Task CreateCommentAsync(Comment comment)
        {
            await _commentRepository.CreateCommentAsync(comment);
        }
        public async Task<Comment?>GetExistingCommentAsync(int id)
        {
            return await _commentRepository.GetExistingCommentAsync(id);
        }
        public async Task UpdateCommentAsync(Comment comment)
        {
            await _commentRepository.UpdateCommentAsync(comment);
        }
        public async Task DeleteCommentAsync(int id)
        {
             await _commentRepository.DeleteCommentAsync(id);
        }
        public async Task<bool> CommentExistsAsync(int id)
        {
            return await _commentRepository.CommentExistsAsync(id);
        }



    }
}
