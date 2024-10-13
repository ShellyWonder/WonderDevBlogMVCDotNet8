using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using X.PagedList;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<IPagedList<Post>> GetAllPostsByStateAsync(PostState postState, int pageNumber, int pageSize, int id);

        Task<IEnumerable<Post>> GetPostsByBlogIdAsync(int id);
        Task<Post> GetPostByIdAsync(int id);
        Task<Post> GetPostBySlugAsync(string slug);
        Task AddPostAsync(Post post, string userId);
        Task UpdatePostAsync(Post post, string userId);
        Task DeletePostAsync(int id);
        Task<bool> PostExistsAsync(int id);
    }
}
