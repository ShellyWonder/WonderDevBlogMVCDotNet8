namespace WonderDevBlogMVC2024.Data.Repositories.Interfaces
{
    public interface ISlugRepository
    {
        public bool IsSlugUnique(string slug);
    }
}
