using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;

namespace WonderDevBlogMVC2024.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string? AuthorId { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "Blog Name")]
        public  required string Name { get; set; }
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        [Display(Name = "Blog Description")]
        public required string Description { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        [Display(Name = "Blog Image")]
        public byte[]? Image { get; set; }

        [Display(Name = "Image Type")]
        public string? ImageType { get; set; }

        // This will be used to upload the file from the form.
        [NotMapped]
        public IFormFile? ImageFile { get; set; }



        //Navigation properties
        //Parent Author to Child Blog
        public virtual ApplicationUser? Author { get; set; }

        //Parent Blog to Child Posts
        public virtual ICollection<Post> Posts { get; set; } = [];
    }
}
