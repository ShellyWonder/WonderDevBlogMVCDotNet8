﻿using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;
using X.PagedList;

namespace WonderDevBlogMVC2024.Services
{
    public class SearchService(ISearchRepository searchRepository) : ISearchService
    {
        private readonly ISearchRepository _searchRepository = searchRepository;

        public async Task<IPagedList<Post>> SearchPosts(PostState postState, int pageNumber, int pageSize, string searchTerm)
        {
            return  await _searchRepository.SearchPostsByStateAsync(postState, pageNumber, pageSize, searchTerm);
        }
    }
}