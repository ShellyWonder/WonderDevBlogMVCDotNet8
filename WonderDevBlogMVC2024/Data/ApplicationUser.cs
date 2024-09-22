using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Enums;

namespace WonderDevBlogMVC2024.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        
        [StringLength(50, ErrorMessage ="The {0} must be at least {2} and no more than {1}", MinimumLength =2)]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Display(Name = "User Profile Image")]
        public byte[]? ImageData { get; set; }

        public ImageType ImageType { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "GitHub Url")]
        public string? GitHubUrl {  get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "LinkedIn Url")]
        public string? LinkdInUrl { get; set; }

        [NotMapped]
        public string? FullName {

            get
            {
                return $"{FirstName} {LastName}";
            }
            set
            {

            }
        }
        // This will be used to upload the file from the form.
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


        //Navigation Properties
        public virtual ICollection<Blog> Blogs { get; set; } = [];
        public virtual ICollection<Post> Posts { get; set; } = [];
    }

}
