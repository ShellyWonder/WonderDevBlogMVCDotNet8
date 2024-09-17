using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IBlogService _blogService;
        private readonly IApplicationUserService _applicationUserService;

        public PostsController(IPostService postService, IBlogService blogService, IApplicationUserService applicationUserService)
        {
            _postService = postService;
            _blogService = blogService;
            _applicationUserService = applicationUserService;
        }



        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllPostsAsync();
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            await PopulateViewDataAsync();
            return View();
        }

        // POST: Posts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,BlogPostState,ImageData")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Created = DateTime.Now;
                await _postService.AddPostAsync(post);
                return RedirectToAction(nameof(Index));
            }
            await PopulateViewDataAsync(post);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            await PopulateViewDataAsync(post);
            return View(post);
        }

        // POST: Posts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,BlogPostState,ImageData")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {   
                post.Updated = DateTime.Now;
                await _postService.UpdatePostAsync(post);
                return RedirectToAction(nameof(Index));
            }
            await PopulateViewDataAsync(post);
            return View(post);
        }


        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeletePostAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PostExists(int id)
        {
            return await _postService.PostExistsAsync(id);
        }

        private async Task PopulateViewDataAsync(Post? post = null)
        {
            {

                //NOTE: CHANGE VAR AUTHOR WHEN ROLES ARE IMPLEMENTED
                var authors = await _applicationUserService.GetAllUsersAsync();
                var blogs = await _blogService.GetAllBlogsAsync();
                //may not use AuthorId
                //ViewData["AuthorId"] = new SelectList(authors, "Id", "Id", post?.AuthorId);
                ViewData["BlogId"] = new SelectList(blogs, "Id", "Name", post?.BlogId);

            }
        }
    }
}
