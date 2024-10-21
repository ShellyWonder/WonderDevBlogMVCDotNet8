using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Data.Repositories
{
    #region PRIMARY CONSTRUCTOR
    public class ApplicationUserRepository(ApplicationDbContext context,
                                            UserManager<ApplicationUser> userManager) : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        #endregion

        #region GET ALL USERS
        public async Task<IEnumerable<UserViewModel?>> GetAllUsersAsync()
        {

            var users = _context.Users
                                             .Select(u => new UserViewModel
                                             {
                                                 Id = u.Id,              // Access the Id from ApplicationUser
                                                 FullName = u.FullName    // Access the FullName from ApplicationUser
                                             });

            return await users.ToListAsync();
        } 
        #endregion

    #region GET USER BY ID
        /// <summary>
        /// ApplicationUser inherits id from IdentityUser whose Id is a guid.
        /// Therefore, GetUserByIdAsync has a string id parameter)
        /// </summary>

        public async Task<UserViewModel?> GetUserByIdAsync(string id)
        {
            // Await the result and store it in a variable
            var user = await _context.Users
                                     .Where(x => x.Id == id)
                                     .Select(x => new UserViewModel
                                     {
                                         Id = x.Id,
                                         FullName = x.FullName
                                     })
                                     .SingleOrDefaultAsync();

            // Check if the user is null and throw an exception if needed
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }

            return user;
        }
        #endregion

    #region GET ALL MODERATORS
        public async Task<IEnumerable<UserViewModel?>> GetAllModeratorsAsync()
        {
            var moderators = _context.Comments
                .Where(c => c.Moderator != null)
                .Select(c => new UserViewModel
                {
                    Id = c.Moderator!.Id,
                    FullName = c.Moderator!.FullName
                })
                .Distinct();

            return await moderators.ToListAsync();
        } 
        #endregion

 #region GET MODERATOR BY ID/FULL NAME
        public async Task<UserViewModel?> GetModeratorByIdAsync(string id)

        {
           var moderator = await _context.Users
                                    //joining Comments 
                                    .Where(u => _context.Comments.Any(c => c.ModeratorId == u.Id && u.Id == id))
                                    .Select(u => new UserViewModel
                                    {
                                        Id = u.Id,
                                        FullName = u.FullName 
                                    })
                                    .SingleOrDefaultAsync();


            // Check if the user is null and throw an exception if needed
            if (moderator == null)
            {
                throw new KeyNotFoundException($"Moderator with ID {id} was not found.");
            }

            return moderator;
        }
        #endregion

    #region GET ALL AUTHORS
        //pulls both BLOG & POST Authors
        public async Task<IEnumerable<UserViewModel?>> GetAllAuthorsAsync()
        {
            var blogAuthors = await GetAllBlogAuthorsAsync();

            var postAuthors = await GetAllPostAuthorsAsync();

            return blogAuthors
            .Union(postAuthors)
            .Distinct();
        }
        #endregion

        #region GET ALL BLOG AUTHORS
        public async Task<IEnumerable<UserViewModel?>> GetAllBlogAuthorsAsync()
        {
            var blogAuthors = _context.Blogs
                .Where(b => b.Author != null)
                .Select(p => new UserViewModel
                {
                    Id = p.Author!.Id,
                    FullName = p.Author!.FullName
                })
                .Distinct();

            return await blogAuthors.ToListAsync();

        }
        #endregion

        #region GET ALL POST AUTHORS
        public async Task<IEnumerable<UserViewModel?>> GetAllPostAuthorsAsync()
        {
            var postAuthors = _context.Posts
                .Where(p => p.Author != null)
                .Select(p => new UserViewModel
                {
                    Id = p.Author!.Id,            
                    FullName = p.Author!.FullName  
                })
                .Distinct();

            return await postAuthors.ToListAsync();
        }
        #endregion
        
#region GET AUTHOR BY ID
        public async Task<UserViewModel?> GetAuthorByIdAsync(string id)
        {
            // Search for the author in Blogs
            var blogAuthor = await GetBlogAuthorByIdAsync(id);
            if (blogAuthor != null)
            {
                return blogAuthor;
            }

            // If not found in Blogs, search for the author in Posts
            return await GetPostAuthorByIdAsync(id);
        }
        #endregion

        #region GET BLOG AUTHOR BY ID
        public async Task<UserViewModel?> GetBlogAuthorByIdAsync(string id)
        {
                // Search for the author in Blogs
            var blogAuthor = await _context.Blogs
                .Where(b => b.Author != null && b.Author.Id == id)
                .Select(b => new UserViewModel
                {
                    Id = b.Author!.Id,
                    FullName = b.Author!.FullName
                })
                .FirstOrDefaultAsync();

                return blogAuthor;
        }
        #endregion

        #region GET POST AUTHOR BY ID
        public async Task<UserViewModel?> GetPostAuthorByIdAsync(string id)
        {
            //search for the author in Posts
            var postAuthor = await _context.Posts
                .Where(p => p.Author != null && p.Author.Id == id)
                .Select(p => new UserViewModel
                {
                    Id = p.Author!.Id,
                    FullName = p.Author!.FullName
                })
                .FirstOrDefaultAsync();
                 return postAuthor; 
        }
        #endregion

        #region GET ALL ADMINISTRATORS
        public async Task<IEnumerable<UserViewModel?>> GetAllAdministratorsAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Administrator");

            var administrators = usersInRole
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName 
                })
                .Distinct();

            return administrators;
        }
        #endregion

        #region GET ADMINISTRATOR BY ID/FULL NAME
        public async Task<UserViewModel?> GetAdministratorByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null && await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName 
                };
            }

            throw new KeyNotFoundException($"Administrator with ID {id} was not found.");
        }
        #endregion

    }

}
    
