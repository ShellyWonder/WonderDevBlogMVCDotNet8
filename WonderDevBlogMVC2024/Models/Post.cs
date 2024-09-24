using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;


namespace WonderDevBlogMVC2024.Models
{
    public class Post
    {
        public int Id { get; set; }
        
        public int BlogId { get; set; }
        public string? AuthorId {  get; set; }

        [Display(Name = "Post Title")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        public required string Title { get; set; }

        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        public required string? Abstract { get; set; }

       [Display(Name ="Post Body")]
       public required string? Content { get; set; }   

        public string? Slug {  get; set; }

        [Display(Name = "Post State")]
        public PostState BlogPostState { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Updated { get; set; }

        public byte[] ImageData { get; set; } = Array.Empty<byte>();

        public string? ImageType { get; set; }

        // This will be used to upload the file from the form.
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


        //Navigation Properties

        public virtual Blog? Blog { get; set; }
        public virtual ApplicationUser? Author { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } = [];

        //Holds all of the comments for a post
        public virtual ICollection<Comment> Comments { get; set; } = [];
    }
}
