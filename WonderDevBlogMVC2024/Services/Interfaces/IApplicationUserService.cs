using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<AuthorViewModel?>> GetAllAuthorsAsync();
                                        
        Task<IEnumerable<AuthorViewModel?>> GetAllBlogAuthorsAsync();
                                        
        Task<IEnumerable<AuthorViewModel?>> GetAllPostAuthorsAsync();


    }  
}
