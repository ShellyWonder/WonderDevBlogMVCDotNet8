using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    #region PRIMARY CONSTRUCTOR
    public class BlogsController(IBlogService blogService,
                 UserManager<ApplicationUser> userManager,
                 IErrorHandlingService errorHandlingService,
                 IImageService imageService) : Controller
    {
        private readonly IBlogService _blogService = blogService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;
        private readonly IImageService _imageService = imageService;
        #endregion

        #region GET BLOGS INDEX
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllBlogsAsync();
            return View(blogs);
        }
        #endregion

        #region GET BLOG DETAILS
        [AllowAnonymous]
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
        #endregion

        #region GET BLOG CREATE
        [Authorize(Roles = "Administrator,Author")]
        public IActionResult Create()
        {

            return View();
        }
        #endregion

        #region POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        //When component is created, replace Image in bind appropriately
        public async Task<IActionResult> Create([Bind("Name,Description,ImageFile")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                // Get the current user ID
                var userId = _userManager.GetUserId(User);

                // Convert the uploaded image to a byte array and the image data
                blog = await ImageImplementationAsync(blog);

                // Pass  blog and userId to the service/repository
                await _blogService.CreateBlogAsync(blog, userId!);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }
        #endregion

        #region GET EDIT
        [Authorize(Roles = "Administrator,Author")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return _errorHandlingService.HandleError($"Blog with ID {id} was not found.");
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return _errorHandlingService.HandleError($"Blog with ID {id} was not found.");
            }
            return View(blog);
        }
        #endregion

        #region POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Blog blog, IFormFile newImage)
        {
            if (id != blog.Id)
            {
                return _errorHandlingService.HandleError($"Blog with ID {blog.Id} was not found.");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // Get the current user ID
                    var userId = _userManager.GetUserId(User);

                    // Get the current blog ID
                    var newBlog = await _blogService.GetBlogByIdAsync(blog.Id);

                    if (newBlog!.Name != blog.Name) newBlog.Name = blog.Name;

                    if (newBlog!.Description != blog.Description) newBlog.Description = blog.Description;

                    if (newBlog!.Name != blog.Name) newBlog.Name = blog.Name;


                    // Check if a new image has been uploaded
                    if (newImage != null)
                    {
                        // Assign the new image to the ImageFile property of the Blog object
                        newBlog.ImageFile = newImage;
                        // Convert the uploaded image to a byte array and store it in the database
                        blog = await ImageImplementationAsync(newBlog);
                    }
                    // Pass userId to the service/repository to update the rest of the blog
                    await _blogService.UpdateBlogAsync(blog, userId!);
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {

                    return _errorHandlingService.HandleError($"Blog with ID {blog.Id} was not found.");
                }

            }
            return View(blog);
        }
        #endregion

        #region GET DELETE
        [Authorize(Roles = "Administrator,Author")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return _errorHandlingService.HandleError($"Blog with ID {id} was not found.");
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return _errorHandlingService.HandleError($"Blog with ID {id} was not found.");
            }

            return View(blog);
        }
        #endregion

        #region POST DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogService.DeleteBlogAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region BLOG EXISTS
        private async Task<bool> BlogExists(int id)
        {
            return await _blogService.BlogExistsAsync(id);
        }
        #endregion

        #region POPULATE AUTHORS
        private async Task PopulateAuthorsDropDownList(object? selectedAuthor = null)
        {
            var authors = await _blogService.GetAllAuthorsAsync();
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName", selectedAuthor);
        }
        #endregion

        #region IMG IMPLEMENTATION
        private async Task<Blog> ImageImplementationAsync(Blog blog)
        {
            if (blog.ImageFile != null)
            {    //Convert incoming file into a byte array
                blog.Image = await _imageService.ConvertFileToByteArrayAsync(blog.ImageFile);
                blog.ImageType = blog.ImageFile.ContentType;
            }
            else
            {
                // Assign default image if no image is uploaded
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "default_icon.png");
                blog.Image = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                blog.ImageType = "image/png";
            }
            return blog;
        }
        #endregion

        
    }
}