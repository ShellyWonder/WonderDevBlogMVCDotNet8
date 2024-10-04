using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class TagService(ITagRepository tagRepository) : ITagService
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllTagsAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _tagRepository.GetTagByIdAsync(id);
        }

        public async Task AddTagAsync(string tagText, int postId, string authorId)
        {
           await _tagRepository.AddTagAsync( tagText, postId,  authorId);
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            await _tagRepository.UpdateTagAsync(tag);
        }

        public async Task DeleteTagAsync(int id)
        {
            await _tagRepository.DeleteTagAsync(id);
        }

        public bool TagExists(int id)
        {
            return _tagRepository.TagExists(id);
        }

       
    }
}
