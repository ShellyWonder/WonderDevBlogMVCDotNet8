﻿using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<UserViewModel?>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<IEnumerable<UserViewModel?>> GetAllAuthorsAsync();
                                        
        Task<IEnumerable<UserViewModel?>> GetAllBlogAuthorsAsync();
        Task<IEnumerable<UserViewModel?>> GetAllPostAuthorsAsync();

        Task<IEnumerable<UserViewModel?>> GetAllModerators();
        Task<UserViewModel?> GetModeratorById(string id);
        Task<UserViewModel?> GetAuthorByIdAsync(string id);
        Task<UserViewModel?> GetBlogAuthorByIdAsync(string id);
        Task<UserViewModel?> GetPostAuthorByIdAsync(string id);
        Task<IEnumerable<UserViewModel?>> GetAllAdministratorsAsync();
        Task<UserViewModel?> GetAdministratorByIdAsync(string id);


    }
}
