using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Services.Interfaces;

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
        public async Task<IEnumerable<string?>> GetAllAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllAuthorsAsync();
        } 
        #endregion
    }
}
