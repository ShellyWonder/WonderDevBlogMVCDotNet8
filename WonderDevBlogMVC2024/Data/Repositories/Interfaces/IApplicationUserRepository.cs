namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        //Task<IEnumerable<ApplicationUser>> GetAllAuthors();
        //Task<ApplicationUser> GetAuthorById(string id);
        //Task<IEnumerable<ApplicationUser>> GetAllModerators();
        //Task<ApplicationUser> GetModeratorById(string id);
        Task<ApplicationUser?> UpdateUserAsync(ApplicationUser updatedUser);
    }
}
