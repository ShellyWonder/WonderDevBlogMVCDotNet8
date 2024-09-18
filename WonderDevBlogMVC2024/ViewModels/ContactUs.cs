using System.ComponentModel.DataAnnotations;

namespace WonderDevBlogMVC2024.ViewModels
{
    public class ContactUs
    {
        public required string Name { get; set; }

        [EmailAddress]
        [StringLength(80, ErrorMessage = "The {0} must be at least{2} and at most {1} characters long.", MinimumLength = 6)]
        public required string Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
        
        [StringLength(100, ErrorMessage = "The {0} must be at least{2} and at most {1} characters long.", MinimumLength = 6)]
        public required string Subject { get; set; }

        [StringLength(750, ErrorMessage = "The {0} must be at least{2} and at most {1} characters long.", MinimumLength = 6)]
        public required string Message { get; set; }
    }
}
