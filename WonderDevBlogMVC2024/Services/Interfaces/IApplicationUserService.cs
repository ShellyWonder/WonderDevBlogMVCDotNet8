using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<UserViewModel?>> GetAllAuthorsAsync();
                                        
        Task<IEnumerable<UserViewModel?>> GetAllBlogAuthorsAsync();
                                        
        Task<IEnumerable<UserViewModel?>> GetAllPostAuthorsAsync();


    }  
}
