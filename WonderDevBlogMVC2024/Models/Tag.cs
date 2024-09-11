using WonderDevBlogMVC2024.Data;


namespace WonderDevBlogMVC2024.Models
{
    //NOTE: Validation annotation is handled in BlogValidator.cs
    //Display annotations are handled on the model
    public class Tag
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string? AuthorId { get; set; }

        public string? Text { get; set; }


        //Navigation properties
        public virtual Post? Post { get; set; }
        public virtual ApplicationUser? Author { get; set; }
    }
}
