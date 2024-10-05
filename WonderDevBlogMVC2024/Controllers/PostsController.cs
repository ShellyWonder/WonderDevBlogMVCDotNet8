using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using WonderDevBlogMVC2024.Areas.Identity.Pages;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    public class PostsController(IPostService postService, IBlogService blogService, 
                                 IApplicationUserService applicationUserService, 
                                 IImageService imageService,
                                 UserManager<ApplicationUser> userManager,
                                 ISlugService slugService, ITagService tagService) : Controller
    {
        private readonly IPostService _postService = postService;
        private readonly IBlogService _blogService = blogService;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;
        private readonly IImageService _imageService = imageService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ISlugService _slugService = slugService;
        private readonly ITagService _tagService = tagService;

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllPostsAsync();
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var post = await _postService.GetPostBySlugAsync(slug);

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
                if (!ModelState.IsValid)
                {
                    await PopulateViewDataAsync(post);
                    ViewData["tagValues"] = string.Join(", ", tagValues);
                    return View(post);
                }
                try
                {
                // Get the current user ID
                var authorId = _userManager.GetUserId(User);
                post.AuthorId = authorId;
                var slug = _slugService.UrlFriendly(post.Title);
                    if (!_slugService.IsUnique(slug))
                    {
                        ModelState.AddModelError("Title", "The title you provided is a duplicate of an existing title. Therefore, it cannot be used again.");
                        ViewData["tagValues"] = string.Join(", ", tagValues);
                        return View(post);
                    }

                    post.Slug = slug;

                    // Convert the uploaded image to a byte array and store it in database
                    post = await ImageImplementationAsync(post);

                foreach (var tagText in tagValues)
                {
                    // Add each tag using the tag service/repository method
                    await _tagService.AddTagAsync(tagText, post.Id, authorId!);
                }


                // Pass userId to the service/repository
                await _postService.AddPostAsync(post, authorId!);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    //  if needed, return a custom error message
                    return HandleError($"An error occurred while creating the post: {ex.Message}");
                }
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
                return HandleError($"Post with ID {id} was not found.");
            }

            await PopulateViewDataAsync(post);
            ViewData["TagValues"] = string.Join(",",post.Tags.Select(t => t.Text));
            return View(post);
        }

        // POST: Posts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,BlogPostState")] Post post, IFormFile newImage)
        {
            if (id != post.Id)
            {
                return HandleError($"Post with ID {post.Id} was not found.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current user ID
                    var userId = _userManager.GetUserId(User);
                    // Get the current blog ID
                    var newPost = await _postService.GetPostByIdAsync(post.Id);

                    if (newPost.Title != newPost.Title) newPost.Title = post.Title;
                    if (newPost.Abstract != newPost.Abstract) newPost.Abstract = post.Abstract;
                    if (newPost.Content != newPost.Content) newPost.Content = post.Content;
                    if (newPost.BlogPostState != newPost.BlogPostState) newPost.BlogPostState = post.BlogPostState;

                    // Check if a new image has been uploaded
                    if (newImage != null)
                    {
                        // Assign the new image to the ImageFile property of the Blog object
                        newPost.ImageFile = newImage;
                        // Convert the uploaded image to a byte array and store it in the database
                        newPost = await ImageImplementationAsync(newPost);
                    }
                    // Pass userId to the service/repository,updating the rest of the post
                    await _postService.UpdatePostAsync(post, userId!);
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    return HandleError($"Post with ID {post.Id} was not found.");
                }
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
        private async Task<Post> ImageImplementationAsync(Post post)
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
            return post;
        }

        private IActionResult HandleError(string errorMessage)
        {
            var errorModel = new ErrorModel
            {
                CustomErrorMessage = errorMessage
            };
            // Return the error view with the message
            return View("Error", errorModel); 
        }
    }
}
