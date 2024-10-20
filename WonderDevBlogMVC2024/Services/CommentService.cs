using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    #region PRIMARY CONSTRUCTOR
    public class CommentService(ICommentRepository commentRepository) : ICommentService
    {
        private readonly ICommentRepository _commentRepository = commentRepository;

        #endregion

    #region GET ALL COMMENTS
        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _commentRepository.GetAllCommentsAsync();
        }
        #endregion

    #region GET COMMENT BY ID
        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _commentRepository.GetCommentByIdAsync(id);
        }
        #endregion

    #region CREATE COMMENT
        public async Task CreateCommentAsync(Comment comment)
        {
            await _commentRepository.CreateCommentAsync(comment);
        }
        #endregion

    #region GET EXISTING COMMENT
        public async Task<Comment?> GetExistingCommentAsync(int id)
        {
            return await _commentRepository.GetExistingCommentAsync(id);
        }
        #endregion

    #region UPDATE COMMENT
        public async Task UpdateCommentAsync(Comment comment)
        {
            await _commentRepository.UpdateCommentAsync(comment);
        }
        #endregion

    #region DELETE COMMENT
        public async Task DeleteCommentAsync(int id)
        {
            await _commentRepository.DeleteCommentAsync(id);
        }
        #endregion

    #region COMMENT EXISTS
        public async Task<bool> CommentExistsAsync(int id)
        {
            return await _commentRepository.CommentExistsAsync(id);
        } 
        #endregion

    }
}
