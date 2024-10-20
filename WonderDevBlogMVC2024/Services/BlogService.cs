using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;
using WonderDevBlogMVC2024.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace WonderDevBlogMVC2024.Services
{
#region PRIMARY CONSTRUCTOR
    public class BlogService(IBlogRepository blogRepository, IApplicationUserService applicationUserService) : IBlogService
    {
        private readonly IBlogRepository _blogRepository = blogRepository;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;

        #endregion

#region GET ALL BLOGS
        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await _blogRepository.GetAllBlogsAsync();
        }
        public async Task<IPagedList<Blog>> GetBlogsByStateAsync(PostState postState, int pageNumber, int pageSize)
        {
            return await _blogRepository.GetBlogsByStateAsync(postState, pageNumber, pageSize);
        } 
        #endregion

#region GET BLOG BY ID
        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _blogRepository.GetBlogByIdAsync(id);
        }
        #endregion

#region CREATE BLOG
        public async Task CreateBlogAsync(Blog blog, string userId)
        {
            await _blogRepository.CreateBlogAsync(blog, userId);
        } 
        #endregion

#region UPDATE BLOG
        public async Task UpdateBlogAsync(Blog blog, string userId)
        {
            await _blogRepository.UpdateBlogAsync(blog, userId);
        } 
        #endregion

#region DELETE BLOG
        public async Task DeleteBlogAsync(int id)
        {
            await _blogRepository.DeleteBlogAsync(id);
        } 
        #endregion

#region BLOG EXISTS 
        public async Task<bool> BlogExistsAsync(int id)
        {
            return await _blogRepository.BlogExistsAsync(id);
        } 
        #endregion

#region GET ALL BLOG AUTHORS
        public async Task<IEnumerable<AuthorViewModel?>> GetAllBlogAuthorsAsync()
        {
            return await _applicationUserService.GetAllBlogAuthorsAsync();
        } 
        #endregion

#region GET ALL AUTHORS
        public async Task<IEnumerable<AuthorViewModel?>> GetAllAuthorsAsync()
        {  
            //pulls down both BLOG & POST authors as potential authors of a new blog.
            return await _applicationUserService.GetAllAuthorsAsync();
        } 
        #endregion


    }
}
