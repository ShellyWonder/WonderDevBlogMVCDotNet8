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
        public async Task<IEnumerable<UserViewModel?>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }
        #endregion

#region GET USER BY ID
        public async Task<UserViewModel?> GetUserByIdAsync(string id)
        {
            return await _applicationUserRepository.GetUserByIdAsync(id);
        } 
        #endregion

#region GET ALL AUTHORS
        //pulls authors from both POSTS & BLOGS tables
        public async Task<IEnumerable<UserViewModel?>> GetAllAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllAuthorsAsync();
        }
        #endregion

#region GET AUTHOR BY ID
        public async Task<UserViewModel?> GetAuthorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetAuthorByIdAsync(id);
        }
        #endregion

#region GET BLOG AUTHOR BY ID
        public async Task<UserViewModel?> GetBlogAuthorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetBlogAuthorByIdAsync(id);
        }

        #endregion

#region GET POST AUTHOR BY ID 
        public async Task<UserViewModel?> GetPostAuthorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetPostAuthorByIdAsync(id);
        }

        #endregion

#region GET ALL ADMINISTRATORS
        public async Task<IEnumerable<UserViewModel?>> GetAllAdministratorsAsync()
        {
            return await _applicationUserRepository.GetAllAdministratorsAsync();
        }
        #endregion

#region GET ADMINISTRATOR BY ID
        public async Task<UserViewModel?> GetAdministratorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetAdministratorByIdAsync(id);
        } 
        #endregion

#region GET ALL BLOG AUTHORS
        //pulls authors from BLOGS tables
        public async Task<IEnumerable<UserViewModel?>> GetAllBlogAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllBlogAuthorsAsync();
        } 
        #endregion
         
 #region GET ALL POST AUTHORS
        //pulls authors from both POSTS 
        public async Task<IEnumerable<UserViewModel?>> GetAllPostAuthorsAsync()
        {
            return await _applicationUserRepository.GetAllPostAuthorsAsync();
        }
        #endregion

#region GET ALL MODERATORS
        public async Task<IEnumerable<UserViewModel?>> GetAllModeratorsAsync()
        {
            return await _applicationUserRepository.GetAllModeratorsAsync();
        }
        #endregion

#region GET MODERATOR BY ID
        public async Task<UserViewModel?> GetModeratorByIdAsync(string id)
        {
            return await _applicationUserRepository.GetModeratorByIdAsync(id);
        }
 
        #endregion

    }
}
