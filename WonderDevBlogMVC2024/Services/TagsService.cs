using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class TagsService(ITagsRepository tagsRepository) : ITagsService
    {
        private readonly ITagsRepository _tagsRepository = tagsRepository;

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagsRepository.GetAllTagsAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _tagsRepository.GetTagByIdAsync(id);
        }
        public async Task AddTagAsync(Tag tag)
        {
           await _tagsRepository.AddTagAsync(tag);
        }
        public async Task UpdateTagAsync(Tag tag)
        {
            await _tagsRepository.UpdateTagAsync(tag);
        }
        public async Task DeleteTagAsync(int id)
        {
            await _tagsRepository.DeleteTagAsync(id);
        }

        public bool TagExists(int id)
        {
            return _tagsRepository.TagExists(id);
        }

       
    }
}
