﻿using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;

namespace WonderDevBlogMVC2024.Data.Repositories
{
    public class PostRepository(ApplicationDbContext context) : IPostRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddPostAsync(Post post, string userId)
        {
            post.Created = DateTime.Now;
            post.AuthorId = userId;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePostAsync(Post post, string userId)
        {
            post.Updated = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Blog)
                .ToListAsync();
        }


        public async Task<Post> GetPostByIdAsync(int id)
        {
            var post = await _context.Posts
                            .Include(p => p.Tags)
                            .FirstOrDefaultAsync(p => p.Id == id);
                            

            if (post == null)
            {

                throw new KeyNotFoundException($"Post with id {id} not found.");

            }

            return post;

        }

        public async Task<Post> GetPostBySlugAsync(string slug)
        {
            var post = await _context.Posts
                           // Eagerly load the author
                           .Include(p => p.Author)
                           // Eagerly load the blog (parent)
                           .Include(p => p.Blog)
                           // Eagerly load the tags
                           .Include(p => p.Tags)     
                           .FirstOrDefaultAsync(p => p.Slug == slug);

            if (post == null)
            {
                throw new KeyNotFoundException($"Post with slug '{slug}' not found.");
            }

            return post;


        }

        public  async Task<bool> PostExistsAsync(int id)
        {
            return await _context.Posts.AnyAsync(p => p.Id == id);
        }

    }
}
