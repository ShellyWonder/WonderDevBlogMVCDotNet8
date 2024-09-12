using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllBlogsAsync();
            return View(blogs);
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public async Task<IActionResult> Create()
        {
            await PopulateAuthorsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,Name,Description,Created,Updated,ImageData,ImageType")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                await _blogService.CreateBlogAsync(blog);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }
        

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }
             await PopulateAuthorsDropDownList();
            return View(blog);
        }

        // POST: Blogs/Edit/5
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,Name,Description,Created,Updated,ImageData,ImageType")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _blogService.UpdateBlogAsync(blog);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogService.DeleteBlogAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BlogExists(int id)
        {
            return await _blogService.BlogExistsAsync(id);
        }

        private async Task PopulateAuthorsDropDownList(object? selectedAuthor = null)
        {
            var authors = await _blogService.GetAllAuthorsAsync();
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName", selectedAuthor);
        }
    }
}
