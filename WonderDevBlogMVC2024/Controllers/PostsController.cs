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
    #region PRIMARY CONSTRUCTOR
    public class PostsController(IPostService postService, IBlogService blogService, 
                                 IApplicationUserService applicationUserService, 
                                 IImageService imageService,
                                 UserManager<ApplicationUser> userManager,
                                 ISlugService slugService, ITagService tagService,
                                 ISearchService searchService,
                                 IErrorHandlingService errorHandlingService) : Controller
    {
        private readonly IPostService _postService = postService;
        private readonly IBlogService _blogService = blogService;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;
        private readonly IImageService _imageService = imageService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ISlugService _slugService = slugService;
        private readonly ITagService _tagService = tagService;
        private readonly ISearchService _searchService = searchService;
        private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;

        #endregion

        #region SEARCH INDEX
        [AllowAnonymous]
        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return _errorHandlingService.HandleError($"The search term '{searchTerm}' was not found. Please try again.");
            }

            ViewData["SearchTerm"] = searchTerm;
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var posts = await _searchService.SearchPosts(PostState.ProductionReady,pageNumber,pageSize, searchTerm);
            return View(posts);
        }

        #endregion

        #region GET POSTS/INDEX
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllPostsAsync();
            return View(posts);
        }
        #endregion

        #region GET ALL BLOG POSTS INDEX BY BLOG
        [AllowAnonymous]
        public async Task<IActionResult>BlogPostIndex(int id, int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var posts = await _postService.GetAllPostsByStateAsync(PostState.ProductionReady, pageNumber, pageSize, id);
            return View(posts);
        }

        #endregion

        #region GET DETAILS  
        [AllowAnonymous]
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
        #endregion

        #region GET CREATE 
        [Authorize(Roles = "Administrator, Author")]
        public async Task<IActionResult> Create()
        {
            await PopulateViewDataAsync();
            return View();
        }
        #endregion

        #region POST CREATE
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

                #region SLUG CREATION AND VALIDATION 
                //
                var slug = _slugService.UrlFriendly(post.Title);

                var validationError = false;

                    //check for blank or malformed slugs
                    if (string.IsNullOrEmpty(slug))
                    {
                        validationError = true;
                        ModelState.AddModelError("", "The title is not valid. Please provide another title.");
                    }
                    //detect incoming duplicate slugs
                   else if (!_slugService.IsUnique(slug))
                    {
                        validationError = true;
                        ModelState.AddModelError("Title", "The title you provided is a duplicate of an existing title. Therefore, it cannot be used again.");
                        ;
                    }

                    if (validationError)
                {
                    ViewData["TagValues"]= string.Join(",",tagValues);

                    return View(post);
                    }

                    post.Slug = slug;
                #endregion

                #region IMAGE HANDLING
                // Convert the uploaded image to a byte array and store it in database
                post = await ImageImplementationAsync(post);
                #endregion

                #region TAG CREATION AND VALIDATION

                foreach (var tagText in tagValues)
                {
                    // Add each tag using the tag service/repository method
                    await _tagService.AddTagAsync(tagText, post.Id, authorId!);
                }

                #endregion 

                #region SAVE
                // Pass userId to the service/repository
                await _postService.AddPostAsync(post, authorId!);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    //  if needed, return a custom error message
                    return _errorHandlingService.HandleError($"An error occurred while creating the post: {ex.Message}");
                }
            #endregion
        }
        #endregion

        #region GET EDIT
        [Authorize(Roles = "Administrator, Author")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return _errorHandlingService.HandleError($"Post with ID {id} was not found.");
            }

            await PopulateViewDataAsync(post);
            PopulateTagValues(post);
            return View(post);
        }
        #endregion

        #region POST EDIT
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogId,Title,Abstract,Content,BlogPostState")]
                                              Post post, IFormFile newImage,List<string>tagValues)
        {
            if (id != post.Id)
            {
                return _errorHandlingService.HandleError($"Post with ID {post.Id} was not found.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current user ID
                    var userId = _userManager.GetUserId(User);
                    // Get current blog ID FROM EXISTING POST
                    var newPost = await _postService.GetPostByIdAsync(post.Id);

                    if (newPost.Title != newPost.Title) newPost.Title = post.Title;
                    if (newPost.Abstract != newPost.Abstract) newPost.Abstract = post.Abstract;
                    if (newPost.Content != newPost.Content) newPost.Content = post.Content;
                    if (newPost.BlogPostState != newPost.BlogPostState) newPost.BlogPostState = post.BlogPostState;

                    #region SLUGS
                    // Generate a new slug from the updated title and validate uniqueness
                    var newSlug = _slugService.UrlFriendly(post.Title);

                    if (newSlug != newPost.Slug &&  _slugService.IsUnique(newSlug))
                    {
                        newPost.Slug = newSlug;
                    }
                    else
                    {
                        ModelState.AddModelError("Title", "This title is a duplicate of a previous post. Please choose another title");
                        PopulateTagValues(post);
                        await PopulateViewDataAsync(post);//Test this line Slugs 13 use EXISTING(newPost) post
                        return View(post);
                    }
                    #endregion

                    #region IMAGE
                    // If a new image is provided, process and update it
                    if (newImage != null)
                    {
                        // Assign the new image to the ImageFile property of the Blog object
                        newPost.ImageFile = newImage;
                        // Convert the uploaded image to a byte array and store it in the database
                        newPost = await ImageImplementationAsync(newPost);
                    #endregion

                    #region TAGS
                    //Remove all tags associated with the post
                    await _tagService.RemoveAllTagsByPostIdAsync(post.Id);

                    // Add the new tags to the post
                    foreach (var tagText in tagValues)
                    {
                        await _tagService.AddTagAsync(tagText, post.Id, userId!);
                    }

                        }
                        #endregion

                    #region SAVE
                    // Pass userId to the service/repository,updating the rest of the post
                    await _postService.UpdatePostAsync(post, userId!);
                    return RedirectToAction(nameof(Index));
                    #endregion
                }
                catch (KeyNotFoundException)
                {
                    return _errorHandlingService.HandleError($"Post with ID {post.Id} was not found.");
                }
            }
            await PopulateViewDataAsync(post);
            return View(post);
        }
        #endregion

        #region GET DELETE
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
        #endregion

        #region POST DELETE
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
        #endregion

        #region POPULATE VIEW DATA
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
        #endregion

        #region POPULATE TAG VALUES
        private void PopulateTagValues(Post post)
        {
            ViewData["TagValues"] = string.Join(",", post.Tags.Select(t => t.Text));
        }

        #endregion

        #region IMAGE IMPLEMENTATION
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
        #endregion
    }
    
}
