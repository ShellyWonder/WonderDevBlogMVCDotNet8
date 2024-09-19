using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class ImageService : IImageService
    {
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public string GetFileType(IFormFile file)
        {
            return file.ContentType;
        }
    }
}
