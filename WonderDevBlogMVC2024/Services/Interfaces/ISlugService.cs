namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface ISlugService
    {
        public string UrlFriendly(string title);
        public bool IsUnique(string slug);
    }
}
