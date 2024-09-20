namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
        Task<byte[]> ConvertFileToByteArrayAsync(string fileName);
        string? DecodeImage(byte[] data, string type);

        string GetFileType(IFormFile file);
        int Size(IFormFile file);
    }
}
