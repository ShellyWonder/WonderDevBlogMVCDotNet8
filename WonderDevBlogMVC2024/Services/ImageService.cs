using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class ImageService : IImageService
    {
        //image delivered to database from the client
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            if (file is null) return null!;
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
       
        //image delivered to database from the client(overload)
        public async Task<byte[]> ConvertFileToByteArrayAsync(string fileName)
        {
            var file =$"{Directory.GetCurrentDirectory()}/wwwroot/img/{fileName}";
            return await  File.ReadAllBytesAsync(file) ;
        }
        //image delivered from database to the client
        public string? DecodeImage(byte[] data, string type)
        {
            if (data is null || type is null) return null;
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";
           
        }

        public string GetFileType(IFormFile file)
        {
            return file.ContentType;
        }

        public int Size(IFormFile file)
        {
            return Convert.ToInt32(file?.Length);
        }
    }
}
