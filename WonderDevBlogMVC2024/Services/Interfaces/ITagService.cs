using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task AddTagAsync(string tagText, int postId, string authorId);
        
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        bool TagExists(int id);
    }
}
