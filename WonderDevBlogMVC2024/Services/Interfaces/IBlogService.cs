using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using X.PagedList;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllBlogsAsync();
        Task<IPagedList<Blog>> GetAllProdBlogsAsync(int pageNumber, int pageSize);
        Task<Blog?> GetBlogByIdAsync(int id);
        Task CreateBlogAsync(Blog blog, string userId);
        Task UpdateBlogAsync(Blog blog, string userId);
        Task DeleteBlogAsync(int id);
        Task<bool> BlogExistsAsync(int id);
        Task<IEnumerable<ApplicationUser>> GetAllAuthorsAsync();



    }
}
