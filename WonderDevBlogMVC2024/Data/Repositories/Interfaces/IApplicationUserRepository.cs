using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<IEnumerable<AuthorViewModel?>> GetAllAuthorsAsync();
                                        
        Task<IEnumerable<AuthorViewModel?>> GetAllBlogAuthorsAsync();
        Task<IEnumerable<AuthorViewModel?>> GetAllPostAuthorsAsync();


        //Task<ApplicationUser> GetAuthorById(string id);
        //Task<IEnumerable<ApplicationUser>> GetAllModerators();
        //Task<ApplicationUser> GetModeratorById(string id);

    }
}
