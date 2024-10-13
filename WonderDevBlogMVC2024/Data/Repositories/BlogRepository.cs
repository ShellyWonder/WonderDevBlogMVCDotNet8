﻿using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using X.PagedList;
using X.PagedList.EF;

namespace WonderDevBlogMVC2024.Data.Repositories
{
    public class BlogRepository(ApplicationDbContext context) : IBlogRepository
    {
        private readonly ApplicationDbContext _context = context;
       
        
        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }
        
        public async Task<bool> BlogExistsAsync(int id)
        {
            return await _context.Blogs.AnyAsync(e => e.Id == id);
        }

        public async Task CreateBlogAsync(Blog blog, string userId)
        {
            blog.Created = DateTime.Now;
            blog.AuthorId = userId;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateBlogAsync(Blog blog, string userId)
        {
            blog.Updated = DateTime.Now;    
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }
        //Note to Dev: no longer needed??
        public async Task<IEnumerable<Blog>> GetAllBlogsAsync()
        {
            return await _context.Blogs.Include(b => b.Author)
                                       .OrderByDescending(b => b.Created)
                                       .ToListAsync();
        }

        public async Task<IPagedList<Blog>> GetBlogsByStateAsync(PostState postState, int pageNumber, int pageSize)
        {
            return await _context.Blogs.Include(b => b.Author)
                                       .Where(b => b.Posts.Any(p => p.BlogPostState == postState))
                                       .OrderByDescending(b => b.Created)
                                       .ToPagedListAsync(pageNumber, pageSize);
        }


    }
}
