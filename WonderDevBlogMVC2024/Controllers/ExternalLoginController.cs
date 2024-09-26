using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using WonderDevBlogMVC2024.Data;

namespace WonderDevBlogMVC2024.Controllers
{
    [AllowAnonymous]
    public class ExternalLoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public IActionResult Challenge(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "ExternalLogin", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl!);
            }
            else
            {
                // Optionally create a new user if they don't have an account
                var user = new ApplicationUser
                {
                    // Maps Google 'GivenName' to ApplicationUser 'FirstName'
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "DefaultFirstName",  // Fallback if claim is missing
                    // Maps Google 'Surname' to ApplicationUser 'LastName'
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "DefaultLastName",
                    // Maps Google 'Email' to ApplicationUser 'UserName'
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    // Maps Google 'Email' to ApplicationUser 'Email'
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email) };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl!);
                    //add alert to let user know he/she is registered
                }
                 return RedirectToPage("/Account/Login");
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }

}
