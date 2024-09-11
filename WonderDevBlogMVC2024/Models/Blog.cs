using System.ComponentModel.DataAnnotations;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;


namespace WonderDevBlogMVC2024.Models
{

    
    public class Blog
    {
        public int Id { get; set; }
        public string? AuthorId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        [Display(Name = "Blog Image")]
        public byte[]? ImageData { get; set; }

        [Display(Name = "Image Type")]
        public ImageType ImageType { get; set; }


        //Navigation properties
        //Parent Author to Child Blog
        public virtual ApplicationUser? Author { get; set; }

        //Parent Blog to Child Posts
        public virtual ICollection<Post> Posts { get; set; } = [];
    }
}
