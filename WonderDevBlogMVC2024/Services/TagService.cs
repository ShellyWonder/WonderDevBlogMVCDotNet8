using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
#region PRIMARY CONSTRUCTOR
    public class TagService(ITagRepository tagRepository) : ITagService
    {
        private readonly ITagRepository _tagRepository = tagRepository; 
        #endregion

#region GET ALL TAGS
        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllTagsAsync();
        }

        #endregion

#region GET TAG BY ID
        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _tagRepository.GetTagByIdAsync(id);
        }

        #endregion

#region ADD TAG(CREATE)
        public async Task AddTagAsync(string tagText, int postId, string authorId)
        {
            await _tagRepository.AddTagAsync(tagText, postId, authorId);
        }

        #endregion

#region UPDATE TAG
        public async Task UpdateTagAsync(Tag tag)
        {
            await _tagRepository.UpdateTagAsync(tag);
        }

        #endregion

#region DELETE TAG
        public async Task DeleteTagAsync(int id)
        {
            await _tagRepository.DeleteTagAsync(id);
        }

        #endregion

#region REMOVE ALL TAGS BY POST ID
        // Removes all tags associated with a specific post
        public async Task RemoveAllTagsByPostIdAsync(int postId)
        {
            await _tagRepository.RemoveAllTagsByPostIdAsync(postId);
        }
        #endregion

#region TAG EXISTS
        public bool TagExists(int id)
        {
            return _tagRepository.TagExists(id);
        }

        #endregion

    }
}
