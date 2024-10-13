using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using X.PagedList;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IPagedList<Post>> SearchPosts(PostState postState, int pageNumber, int pageSize, string searchTerm);
    }
}