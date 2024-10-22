using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Data;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Services.Interfaces;


namespace WonderDevBlogMVC2024.Services
{
    #region PRIMARY CONSTRUCTOR
    public class RolesService(UserManager<ApplicationUser> userManager,
                         RoleManager<ApplicationUser> roleManager,
                         IErrorHandlingService errorHandlingService,
                         ApplicationDbContext context) : IRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationUser> _roleManager = roleManager;
        private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;
        private readonly ApplicationDbContext _context = context;

        #endregion

        #region ADD USER TO ROLE
        public async Task<bool> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GET ROLE NAME BY ID
        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GET ROLES
        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            try
            {
                List<IdentityRole> result = new();
                result = await _context.Roles.ToListAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GET USER ROLES
        public Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GET USERS NOT IN ROLE
        public async Task<List<ApplicationUser>> GetUsersNotInRoleAsync(BlogRole role)
        {
            try
            {
                // Convert BlogRole enum to string for role lookup
                string roleName = role.ToString();

                // Get users already in the specified role
                List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();

                // Get users not in the specified role
                List<ApplicationUser> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList();

                return roleUsers;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region IS USER IN ROLE

        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName)
        {
            try
            {
                bool result = await _userManager.IsInRoleAsync(user, roleName);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region REMOVE USER FROM ROLE
        public async Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string roleName)
        {
            try
            {
                bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region REMOVE USER FROM ROLES
        public async Task<bool> RemoveUserFromRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            if (user != null)
            {
                try
                {
                    bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return false;
        }
        #endregion
    }
}
