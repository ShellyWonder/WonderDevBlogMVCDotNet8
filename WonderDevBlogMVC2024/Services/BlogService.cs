using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IApplicationUserService _applicationUserService;

        public BlogService(IBlogRepository blogRepository, IApplicationUserService applicationUserService)
        {
            _blogRepository = blogRepository;
            _applicationUserService = applicationUserService;
            
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await _blogRepository.GetAllBlogsAsync();
        }

        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _blogRepository.GetBlogByIdAsync(id);
        }

        public async Task CreateBlogAsync(Blog blog)
        {
            await _blogRepository.CreateBlogAsync(blog);
        }

        public async Task UpdateBlogAsync(Blog blog)
        {
            await _blogRepository.UpdateBlogAsync(blog);
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
