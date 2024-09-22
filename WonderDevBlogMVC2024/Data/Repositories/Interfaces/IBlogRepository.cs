using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<Blog?> GetBlogByIdAsync(int id);
        Task CreateBlogAsync(Blog blog, string userId);
        Task UpdateBlogAsync(Blog blog, string userId);
        Task DeleteBlogAsync(int id);
        Task<bool> BlogExistsAsync(int id);

        
    }
}
