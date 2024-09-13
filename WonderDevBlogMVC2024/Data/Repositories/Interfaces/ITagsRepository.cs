using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface ITagsRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task AddTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        bool TagExists(int id);
    }
}
