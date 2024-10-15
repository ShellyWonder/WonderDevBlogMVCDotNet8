using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    #region PRIMARY CONSTRUCTOR
    public class CommentsController(ICommentService commentService,
                              IApplicationUserService applicationUserService,
                              UserManager<ApplicationUser> userManager,
                              IPostService postService,
                              IErrorHandlingService errorHandlingService) : Controller
    {
        private readonly ICommentService _commentService = commentService;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IPostService _postService = postService;
        private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;
        #endregion

        #region GET COMMENTS
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _commentService.GetAllCommentsAsync());
        }
        #endregion

        #region GET DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentService.GetCommentByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }
        #endregion

        #region GET CREATE
        [Authorize(Roles = "Commentator")]
        public async Task<IActionResult> Create()
        {
            await PopulateSelectLists();
            return View();
        }
        #endregion

        #region POST CREATE
        //NOTE: comment author = Commentator
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Body")]Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Capture the user's ID and assign it to the CommentatorId
                comment.CommentatorId = _userManager.GetUserId(User);
                comment.Created = DateTime.Now;
                await _commentService.CreateCommentAsync(comment);
                return RedirectToAction(nameof(Index));
            }
            await PopulateSelectLists();
            return View(comment);
        }
        #endregion

        #region GET EDIT
        [Authorize(Roles = "Commentator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return _errorHandlingService.HandleError($"Comment with ID {id} was not found.");
            }

            var comment = await _commentService.GetCommentByIdAsync(id.Value);
            if (comment == null)
            {
                return _errorHandlingService.HandleError($"Comment with ID {id} was not found.");
            }
            await PopulateSelectLists();
            return View(comment);
        }
        #endregion

        #region POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body")] Comment comment)
        {
            if (id != comment.Id)
            {
                return _errorHandlingService.HandleError($"Comment with ID {id} was not found.");
            }

            if (ModelState.IsValid)
            {
                var existingComment = await _commentService.GetExistingCommentAsync(id);
                if (existingComment == null)
                {
                    return _errorHandlingService.HandleError($"Comment with ID {id} was not found.");
                }
                try
                {
                    existingComment!.Body = comment.Body;
                    existingComment.Updated = DateTime.Now;
                    await _commentService.UpdateCommentAsync(comment);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CommentExists(comment.Id))
                    {
                        return _errorHandlingService.HandleError($"Comment with ID {id} was not found.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = existingComment.Post!.Slug }, "commentSection");
            }
            await PopulateSelectLists();
            return View(comment);
        }
        #endregion

        #region GET DELETE
        [Authorize(Roles ="Moderator, Commentator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return _errorHandlingService.HandleError($"Comment with ID {id} was not found.");
            }

            var comment = await _commentService.GetCommentByIdAsync(id.Value);
            if (comment == null)
            {
                return _errorHandlingService.HandleError($"Comment with ID {id} was not found.");
            }

            return View(comment);
        }
        #endregion

        #region POST DELETE        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region COMMENT EXISTS
        private async Task<bool> CommentExists(int id)
        {
            return await _commentService.CommentExistsAsync(id);
        }
        #endregion

        #region POPULATE SELECT LISTS
        private async Task PopulateSelectLists(Comment? comment = null)
        {
            //NOTE: CHANGE VAR DEFINITIONS WHEN ROLES ARE IMPLEMENTED
            var authors = await _applicationUserService.GetAllUsersAsync();
            var moderators = await _applicationUserService.GetAllUsersAsync();
            var commentators = await _applicationUserService.GetAllUsersAsync();
            var posts = await _postService.GetAllPostsAsync();

            ViewData["CommentatorId"] = new SelectList(commentators, "Id", "Id", comment?.CommentatorId);
            ViewData["AuthorId"] = new SelectList(authors, "Id", "Id", comment?.AuthorId);
            ViewData["ModeratorId"] = new SelectList(moderators, "Id", "Id", comment?.ModeratorId);
            ViewData["PostId"] = new SelectList(posts, "Id", "Id", comment?.PostId);
        }
        #endregion
    }
}
