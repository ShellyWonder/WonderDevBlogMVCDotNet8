using System.ComponentModel.DataAnnotations;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;


namespace WonderDevBlogMVC2024.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string? AuthorId {  get; set; }
        public string? Title { get; set; }

        public string? Abstract { get; set; }
        public string? Content { get; set; }

        public string? Slug {  get; set; }

        [Display(Name = "Post State")]
        public PostState BlogPostState { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        [Display(Name = "Select Image Type")]
        public ImageType ImageType { get; set; }

        //Navigation Properties

        public virtual Blog? Blog { get; set; }
        public virtual ApplicationUser? Author { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = [];

        //Holds all of the comments for a post
        public virtual ICollection<Comment> Comments { get; set; } = [];
    }
}
