using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<UserViewModel?>> GetAllUsersAsync();
        Task<UserViewModel?> GetUserByIdAsync(string id);
        Task<IEnumerable<UserViewModel?>> GetAllAuthorsAsync();
                                        
        Task<IEnumerable<UserViewModel?>> GetAllBlogAuthorsAsync();
        Task<IEnumerable<UserViewModel?>> GetAllPostAuthorsAsync();

        Task<IEnumerable<UserViewModel?>> GetAllModeratorsAsync();
        Task<UserViewModel?> GetModeratorByIdAsync(string id);
        Task<UserViewModel?> GetAuthorByIdAsync(string id);
        Task<UserViewModel?> GetBlogAuthorByIdAsync(string id);
        Task<UserViewModel?> GetPostAuthorByIdAsync(string id);
        Task<IEnumerable<UserViewModel?>> GetAllAdministratorsAsync();
        Task<UserViewModel?> GetAdministratorByIdAsync(string id);


    }
}
