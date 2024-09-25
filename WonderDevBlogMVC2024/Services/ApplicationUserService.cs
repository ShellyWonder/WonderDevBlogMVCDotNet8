using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class ApplicationUserService(IApplicationUserRepository applicationUserRepository) : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository = applicationUserRepository;
        

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }

        
    }
}
