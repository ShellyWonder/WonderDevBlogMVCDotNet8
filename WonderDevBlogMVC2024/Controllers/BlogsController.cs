using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    public class BlogsController(IBlogService blogService, UserManager<ApplicationUser> userManager, IImageService imageService) : Controller
    {
        private readonly IBlogService _blogService = blogService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IImageService _imageService = imageService;


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
        [Authorize]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //When component is created, replace Image in bind appropriately
        public async Task<IActionResult> Create([Bind("Name,Description,ImageFile")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                // Get the current user ID
                var userId = _userManager.GetUserId(User);
                // Convert the uploaded image to a byte array and store it in Image
                if (blog.ImageFile != null)
                {
                    blog.Image = await _imageService.ConvertFileToByteArrayAsync(blog.ImageFile);
                    blog.ImageType = blog.ImageFile.ContentType;
                }
                // Pass userId to the service/repository
                await _blogService.CreateBlogAsync(blog,userId!);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }


        // GET: Blogs/Edit/5
        [Authorize]
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
            return View(blog);
        }

        // POST: Blogs/Edit/5
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageData")] Blog blog)
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
