using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Enums;
using X.PagedList;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{

    public interface ISearchRepository
    {
        Task<IPagedList<Post>> SearchPostsByStateAsync(PostState postState, int pageNumber, int pageSize, string searchTerm);

    }
}
