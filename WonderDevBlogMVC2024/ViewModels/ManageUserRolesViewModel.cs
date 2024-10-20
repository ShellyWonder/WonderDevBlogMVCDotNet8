using WonderDevBlogMVC2024.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace WonderDevBlogMVC2024.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public ApplicationUser? ApplicationUser { get; set; }

        public MultiSelectList? Roles { get; set; }

        public List<string>? SelectedRoles { get; set; }
    }
}
