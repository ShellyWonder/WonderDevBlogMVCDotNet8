using WonderDevBlogMVC2024.Data;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task UpdateUserProfileImage(string userId, IFormFile file);
    }  
}
