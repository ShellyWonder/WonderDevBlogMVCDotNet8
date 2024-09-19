namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        string GetFileType(IFormFile file);
    }
}
