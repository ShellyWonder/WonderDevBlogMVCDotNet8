using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task UpdatePostImage(int postId, IFormFile file);
        Task DeletePostAsync(int id);
        Task<bool> PostExistsAsync(int id);
    }
}
