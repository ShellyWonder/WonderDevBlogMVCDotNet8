using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class PostService(IPostRepository postRepository) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;
        

        public async Task AddPostAsync(Post post, string userId)
        {
            await _postRepository.AddPostAsync(post, userId);
        }
        public async Task UpdatePostAsync(Post post, string userId)
        {
            await _postRepository.UpdatePostAsync(post, userId);
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
        public async Task<Post> GetPostBySlugAsync(string slug)
        {
            return await _postRepository.GetPostBySlugAsync(slug);
        }

        public  async Task<bool> PostExistsAsync(int id)
        {
            return await _postRepository.PostExistsAsync(id);
        }

    }
}
