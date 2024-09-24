using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    public class PostsController(IPostService postService, IBlogService blogService, 
                                 IApplicationUserService applicationUserService, 
                                 IImageService imageService,
                                 UserManager<ApplicationUser> userManager,
                                 ISlugService slugService) : Controller
    {
        private readonly IPostService _postService = postService;
        private readonly IBlogService _blogService = blogService;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;
        private readonly IImageService _imageService = imageService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ISlugService _slugService = slugService;


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
        [Authorize]
        public async Task<IActionResult> Create()
        {
            await PopulateViewDataAsync();
            return View();
        }

        // POST: Posts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,BlogPostState,ImageFile")] Post post,List<string>tagValues)
        {
            if (ModelState.IsValid)
            {
                // Get the current user ID
                var userId = _userManager.GetUserId(User);
                var slug = _slugService.UrlFriendly(post.Title);
                if (!_slugService.IsUnique(slug))
                {
                    ModelState.AddModelError("Title", "The title you provided is a duplicate of an existing title. Therefore, it cannot be used again.");
                    ViewData["tagValues"] = string.Join(", ", tagValues);
                    return View(post);
                }

                    post.Slug = slug;
 
                // Convert the uploaded image to a byte array and store it in database
                await ImageImplementation(post);
                // Pass userId to the service/repository
                await _postService.AddPostAsync(post,userId!);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,BlogPostState,ImageFile")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Get the current user ID
                var userId = _userManager.GetUserId(User);

                // Check if a new image has been uploaded
                if (post.ImageFile != null)
                {
                    // Convert the uploaded image to a byte array and store it in the database
                    await ImageImplementation(post);
                }
                // Pass userId to the service/repository,updating the rest of the post
                await _postService.UpdatePostAsync(post, userId!);
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
        private async Task ImageImplementation(Post post)
        {
            if (post.ImageFile != null)
            {
                post.ImageData = await _imageService.ConvertFileToByteArrayAsync(post.ImageFile);
                post.ImageType = post.ImageFile.ContentType;
            }
            else
            {
                // Assign default image if no image is uploaded
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "default_icon.png");
                post.ImageData = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                post.ImageType = "image/png";
            }
        }
    }
}
