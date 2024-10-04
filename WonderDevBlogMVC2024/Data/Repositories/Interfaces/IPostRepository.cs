using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<Post> GetPostBySlugAsync(string slug);
        Task AddPostAsync(Post post, string userId);
        Task UpdatePostAsync(Post post, string userId);
        Task DeletePostAsync(int id);
        Task<bool> PostExistsAsync(int id);
    }
}
