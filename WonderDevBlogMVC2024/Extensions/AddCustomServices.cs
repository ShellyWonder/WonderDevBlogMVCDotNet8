using Microsoft.AspNetCore.Http.Features;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Data.Repositories;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Services.Interfaces;
using WonderDevBlogMVC2024.Services;
using WonderDevBlogMVC2024.ViewModels;
using WonderDevBlogMVC2024.Data.SeedData;

namespace WonderDevBlogMVC2024.Extensions
{
    public static class ServicesConfigurationExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddScoped<IBlogEmailSender, EmailSender>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ISlugRepository, SlugRepository>();
            services.AddScoped<ISlugService, SlugService>();
            //data seed services
            services.AddTransient<RolesDataService>();
            services.AddTransient<BlogsDataService>();
            services.AddTransient<PostsDataService>();

            // Configure form options
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024 * 1024 * 10; // 10MB
            });

            return services;
        }
    }

}
