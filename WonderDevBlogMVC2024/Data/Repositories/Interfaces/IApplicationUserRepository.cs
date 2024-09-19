// IApplicationUserRepository.cs
using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        //Task<IEnumerable<ApplicationUser>> GetAllAuthors();
        //Task<IEnumerable<ApplicationUser>> GetAuthorById();
        //Task<IEnumerable<ApplicationUser>> GetAllModerators();
        //Task<IEnumerable<ApplicationUser>> GetModeratorById();

    }
}
