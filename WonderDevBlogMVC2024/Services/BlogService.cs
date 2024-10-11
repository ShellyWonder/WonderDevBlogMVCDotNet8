using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;
using X.PagedList;
using X.PagedList.EF;

namespace WonderDevBlogMVC2024.Services
{
    public class BlogService(IBlogRepository blogRepository, IApplicationUserService applicationUserService) : IBlogService
    {
        private readonly IBlogRepository _blogRepository = blogRepository;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;
        

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await _blogRepository.GetAllBlogsAsync();
        }
        public async Task<IPagedList<Blog>> GetAllProdBlogsAsync(int pageNumber, int pageSize)
        {
            return await _blogRepository.GetAllProdBlogsAsync(pageNumber, pageSize);
        }

        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _blogRepository.GetBlogByIdAsync(id);
        }

        public async Task CreateBlogAsync(Blog blog, string userId)
        {
            await _blogRepository.CreateBlogAsync(blog,userId);
        }

        public async Task UpdateBlogAsync(Blog blog, string userId)
        {
            await _blogRepository.UpdateBlogAsync(blog, userId);
        }

        public async Task DeleteBlogAsync(int id)
        {
            await _blogRepository.DeleteBlogAsync(id);
        }

        public async Task<bool> BlogExistsAsync(int id)
        {
            return await _blogRepository.BlogExistsAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAuthorsAsync()
        {
            return await _applicationUserService.GetAllUsersAsync();
        }

        
    }
}
