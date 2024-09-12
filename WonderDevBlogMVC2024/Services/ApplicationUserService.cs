using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }
    }
}
