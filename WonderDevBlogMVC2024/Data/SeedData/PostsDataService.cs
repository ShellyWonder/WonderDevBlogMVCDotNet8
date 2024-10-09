using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Models;
using global::WonderDevBlogMVC2024.Enums;

namespace WonderDevBlogMVC2024.Data.SeedData
{

    public class PostsDataService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task SeedPostsAndComments()
        {
            if (context.Posts.Any())
            {
                return; // Posts already seeded
            }

            var cssBlog = context.Blogs.FirstOrDefault(b => b.Name == "CSS Tips and Tricks");
            var dotnetBlog = context.Blogs.FirstOrDefault(b => b.Name == "Best Front Development Frameworks for .NET developers");
            var jobMarketBlog = context.Blogs.FirstOrDefault(b => b.Name == "How to use the 12-week year system to prepare for the Web Development Job Market");

            // Seed Posts
                var posts = new List<Post>
                {
                    new ()
                    {
                        Title = "Mastering Flexbox for Responsive Design",
                        Abstract = "Learn how to use Flexbox effectively for modern web design.",
                        Content = "Flexbox is an essential tool in the web developer's toolbox for creating responsive layouts. "
                                + "It simplifies the process of aligning and distributing elements in a container, even when their size is dynamic or unknown. "
                                + "In this guide, we will cover the basics of Flexbox and explore advanced techniques to master responsive design. "
                                + "With Flexbox, you can create layouts that adapt to different screen sizes without the need for complex media queries. "
                                + "We will start with simple layouts and gradually move to more complex examples that demonstrate the full power of Flexbox.",
                        Created = DateTime.UtcNow,
                        BlogId = cssBlog?.Id ?? 0,
                        // Shelly.Wonder
                        AuthorId = "f0c37325-5c21-47a7-a5d9-fc6b03cbd187", 
                        BlogPostState = PostState.ProductionReady
                    },
                    new ()
                    {
                        Title = "Why Blazor is the Best Choice for .NET Developers",
                        Abstract = "Discover why Blazor is revolutionizing web development for .NET developers.",
                        Content = "Blazor is a relatively new framework from Microsoft that allows developers to use C# for both the front-end and back-end of web applications. "
                                + "It eliminates the need for JavaScript and offers a familiar programming model for .NET developers. "
                                + "Blazor can run in the browser via WebAssembly or server-side, offering flexibility depending on the project's needs. "
                                + "In this post, we'll look at the benefits of Blazor, its architecture, and why it's becoming the go-to choice for many .NET developers.",
                        Created = DateTime.UtcNow,
                        BlogId = dotnetBlog?.Id ?? 0,
                        AuthorId = "f0c37325-5c21-47a7-a5d9-fc6b03cbd187",
                        BlogPostState = PostState.ProductionReady
                    },
                    new ()
                    {
                        Title = "Preparing for Your First Web Development Job",
                        Abstract = "How the 12-week year system can boost your preparation for landing a web development job.",
                        Content = "Landing your first web development job can be challenging, especially if you're not sure where to focus your efforts. "
                                + "The 12-week year system provides a framework to set clear goals and track your progress, allowing you to achieve more in less time. "
                                + "In this article, we'll break down how to apply the 12-week year principles to your job search and skill development. "
                                + "By the end of this post, you'll have a roadmap to follow, giving you the confidence to navigate the job market successfully.",
                        Created = DateTime.UtcNow,
                        BlogId = jobMarketBlog?.Id ?? 0,
                        AuthorId = "f0c37325-5c21-47a7-a5d9-fc6b03cbd187",
                        BlogPostState = PostState.ProductionReady
                    }
                };

                context.Posts.AddRange(posts);
                await context.SaveChangesAsync();

                // Get post ids after they are saved
                var flexboxPostId = posts[0].Id;
                var blazorPostId = posts[1].Id;
                var jobPrepPostId = posts[2].Id;

            // Seed Comments for each post
            var comments = new List<Comment>
            {
                // Flexbox Post comments
                new ()
                {
                    PostId = flexboxPostId,
                    CommentorId = "5fb69d7f-31e4-4fae-9d65-5b9b9eea5fd7", // Doug Michaels
                    Body = "Great article! I've always found Flexbox tricky, but this explanation is very clear. Thanks for sharing!",
                    Created = DateTime.UtcNow
                },
                new ()
                {
                    PostId = flexboxPostId,
                    CommentorId = "ce8fcfde-5131-4b16-8d86-1f0ec92ac0d5", // Jack Frost
                    Body = "I agree with Doug! Flexbox is much easier to understand with this guide. I’ve already applied some of these tips in my project.",
                    Created = DateTime.UtcNow
                },

                // Blazor Post comments
                new ()
                {
                    PostId = blazorPostId,
                    CommentorId = "5fb69d7f-31e4-4fae-9d65-5b9b9eea5fd7",
                    Body = "Blazor is fantastic! This post really clarifies why it’s a great tool for .NET developers. I’ve started using it recently and love it.",
                    Created = DateTime.UtcNow
                },
                new ()
                {
                    PostId = blazorPostId,
                    CommentorId = "ce8fcfde-5131-4b16-8d86-1f0ec92ac0d5",
                    Body = "Thanks for this post! I’ve been considering Blazor for a while, and now I’m convinced it’s the right choice for my next project.",
                    Created = DateTime.UtcNow
                },

                // Job Preparation Post comments
                new ()
                {
                    PostId = jobPrepPostId,
                    CommentorId = "5fb69d7f-31e4-4fae-9d65-5b9b9eea5fd7",
                    Body = "The 12-week year system sounds like a great approach. I’ve struggled with job preparation in the past, and this seems like a good way to stay organized.",
                    Created = DateTime.UtcNow
                },
                new ()
                {
                    PostId = jobPrepPostId,
                    CommentorId = "ce8fcfde-5131-4b16-8d86-1f0ec92ac0d5",
                    Body = "I’ve used a similar system, and it really helped me focus my efforts. Great advice here!",
                    Created = DateTime.UtcNow
                }
            };

            context.Comments.AddRange(comments);
            await context.SaveChangesAsync();
        }
    }
}


