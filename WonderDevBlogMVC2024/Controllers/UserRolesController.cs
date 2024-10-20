using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Services;
using WonderDevBlogMVC2024.Services.Interfaces;
using WonderDevBlogMVC2024.ViewModels;


namespace WonderDevBlogMVC2024.Controllers
{
    public class UserRolesController(IRolesService rolesService,
                                     IApplicationUserService ApplicationUserService,
                                     UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IRolesService _rolesService = rolesService;
        private readonly IApplicationUserService _applicationUserService = ApplicationUserService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IActionResult> ManageUserRoles()
        {
            //add instance of the view model
            List<ManageUserRolesViewModel> model = new();

            //Get list of all users
            List<ApplicationUser> users = (List<ApplicationUser>)await _applicationUserService.GetAllUsersAsync();

            foreach (ApplicationUser user in users)
            {
                ManageUserRolesViewModel viewModel = new();
                viewModel.ApplicationUser = user;
            }

            return View(model);
        }
        
        
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AssignRole(string userId, BlogRole role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roleName = role.ToString();
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                // Success message, redirect to user management page
                return RedirectToAction("ManageUsers");
            }

            // Handle errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(); // return to an appropriate view
        }

    }
}
