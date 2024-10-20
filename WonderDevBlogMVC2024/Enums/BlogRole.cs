namespace WonderDevBlogMVC2024.Enums
{
    public enum BlogRole
    {

        Administrator,
        Moderator,
        //blog post author permissions
        Author,
        //Commentator permissions for all comments 
        //create and read, may update own comments but no others
        Commentator
    }
}
