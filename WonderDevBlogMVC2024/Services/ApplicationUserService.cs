using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Services.Interfaces;
using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Services
{
 #region PRIMARY CONSTRUCTOR
    public class ApplicationUserService(IApplicationUserRepository applicationUserRepository) : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository = applicationUserRepository; 
        #endregion

 #region GET ALL USERS 
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }
        #endregion

 #region GET ALL AUTHORS
        //pulls authors from both POSTS & BLOGS tables
        public async Task<IEnumerable<AuthorViewModel?>> GetAllAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllAuthorsAsync();
        } 
        #endregion 

 #region GET ALL BLOG AUTHORS
        //pulls authors from BLOGS tables
        public async Task<IEnumerable<AuthorViewModel?>> GetAllBlogAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllBlogAuthorsAsync();
        } 
        #endregion
         
 #region GET ALL POST AUTHORS
        //pulls authors from both POSTS 
        public async Task<IEnumerable<AuthorViewModel?>> GetAllPostAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllPostAuthorsAsync();
        } 
        #endregion

    }
}
