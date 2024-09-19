using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class ApplicationUserService(IApplicationUserRepository applicationUserRepository, IImageService imageService) : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository = applicationUserRepository;
        private readonly IImageService _imageService = imageService;

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }

        public  async Task UpdateUserProfileImage(string userId, IFormFile file)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(userId) ?? throw new Exception("User not found");
            var imageData = await _imageService.ConvertFileToByteArrayAsync(file);
            var imageType = _imageService.GetFileType(file);

            user.ImageData = imageData;
            user.ImageType = Enum.Parse<ImageType>(imageType, true);

            await _applicationUserRepository.UpdateUserImageAsync(user);
        }
    }
}
