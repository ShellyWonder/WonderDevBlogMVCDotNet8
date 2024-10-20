using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class ImageService : IImageService
    {
    #region CONVERT FILE TO BYTE ARRAY
        //image delivered to database from the client
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            if (file is null) return null!;
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        #endregion

    #region CONVERT FILE TO BYTE ARRAY(OVERLOAD)
        //image delivered to database from the client(overload)
        public async Task<byte[]> ConvertFileToByteArrayAsync(string fileName)
        {
            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/img/{fileName}";
            return await File.ReadAllBytesAsync(file);
        } 
        #endregion

    #region DECODE IMAGE
        //image delivered from database to the client
        public string? DecodeImage(byte[] data, string type)
        {
            if (data is null || type is null) return null;
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";

        } 
        #endregion

    #region GET FILE TYPE
        public string GetFileType(IFormFile file)
        {
            return file.ContentType;
        } 
        #endregion

    #region MANAGE FILE SIZE
        public int Size(IFormFile file)
        {
            return Convert.ToInt32(file?.Length);
        } 
        #endregion

    }
}
