using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPostService _postService;
       
        public CommentsController(ICommentService commentService,
                                  IApplicationUserService applicationUserService,
                                  IPostService postService)
        {
            _commentService = commentService;
            _applicationUserService = applicationUserService;
           _postService = postService;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            return View(await _commentService.GetAllCommentsAsync());
        }

        // GET: Comments/Details/5
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

        // GET: Comments/Create
        public async Task<IActionResult> Create()
        {
            await PopulateSelectLists();
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,PostId,ModeratorId,Body,Created,Updated,Moderated,Deleted,ModeratedBody,ModerationReason")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _commentService.CreateCommentAsync(comment);
                return RedirectToAction(nameof(Index));
            }
            await PopulateSelectLists();
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            await PopulateSelectLists();
            return View(comment);
        }

        // POST: Comments/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,PostId,ModeratorId,Body,Created,Updated,Moderated,Deleted,ModeratedBody,ModerationReason")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _commentService.UpdateCommentAsync(comment);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateSelectLists();
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CommentExists(int id)
        {
            return await _commentService.CommentExistsAsync(id);
        }

        private async Task PopulateSelectLists(Comment? comment = null)
        {
            //NOTE: CHANGE VAR DEFINITIONS WHEN ROLES ARE IMPLEMENTED
            var authors = await _applicationUserService.GetAllUsersAsync();
            var moderators = await _applicationUserService.GetAllUsersAsync();
            var posts = await _postService.GetAllPostsAsync();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "Id", comment?.AuthorId);
            ViewData["ModeratorId"] = new SelectList(moderators, "Id", "Id", comment?.ModeratorId);
            ViewData["PostId"] = new SelectList(posts, "Id", "Id", comment?.PostId);
        }
    }
}
