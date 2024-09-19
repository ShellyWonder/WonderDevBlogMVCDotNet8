using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class PostService(IPostRepository postRepository, IImageService imageService) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IImageService _imageService = imageService;

        public async Task AddPostAsync(Post post)
        {
            await _postRepository.AddPostAsync(post);
        }

        public  async Task DeletePostAsync(int id)
        {
            await _postRepository.DeletePostAsync(id);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _postRepository.GetAllPostsAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetPostByIdAsync(id);
        }

        public  async Task<bool> PostExistsAsync(int id)
        {
            return await _postRepository.PostExistsAsync(id);
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _postRepository.UpdatePostAsync(post);
        }

        public  async Task UpdatePostImage(int postId, IFormFile file)
        {
            //null check using coalesce operator
            var post = await _postRepository.GetPostByIdAsync(postId) ?? throw new Exception("Post not found");
            var imageData = await _imageService.ConvertFileToByteArrayAsync(file);
            var imageType = _imageService.GetFileType(file);

            post.ImageData = imageData;
            post.ImageType = Enum.Parse<ImageType>(imageType, true);

            await _postRepository.UpdatePostAsync(post);
        }
    }
}
