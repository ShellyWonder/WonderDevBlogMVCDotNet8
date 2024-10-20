using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Controllers
{
    #region PRIMARY CONSTRUCTOR
    public class TagsController(ITagService tagsService,
                            IApplicationUserService applicationUserService,
                             IPostService postService) : Controller
    {
        
        private readonly ITagService _tagService = tagsService;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;
        private readonly IPostService _postService = postService;
        #endregion

    #region GET TAGS
        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }
        #endregion

    #region GET DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _tagService.GetTagByIdAsync(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }
        #endregion

    #region GET CREATE
        [Authorize(Roles = "Author, Administrator")]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownLists();
            return View();
        }
        #endregion

    #region POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,AuthorId,TagText")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _tagService.AddTagAsync(tag.Text!, tag.PostId, tag.AuthorId!);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownLists();
            return View(tag);
        }
        #endregion

    #region GET EDIT
        [Authorize(Roles = "Author, Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _tagService.GetTagByIdAsync(id.Value);

            if (tag == null)
            {
                return NotFound();
            }
            await PopulateDropDownLists();
            return View(tag);
        }
        #endregion

    #region POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,AuthorId,Text")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _tagService.UpdateTagAsync(tag);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
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
            await PopulateDropDownLists();
            return View(tag);
        }
        #endregion

    #region GET DELETE
        [Authorize(Roles = "Author, Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _tagService.GetTagByIdAsync(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }
        #endregion

    #region POST DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

    #region TAG EXISTS
        private bool TagExists(int id)
        {
            return _tagService.TagExists(id);
        }
        #endregion

    #region POPULATE DROP DOWN LISTS
        private async Task PopulateDropDownLists(Tag? tag = null)
        {
            
            var authors = await _applicationUserService.GetAllPostAuthorsAsync();
            var posts = await _postService.GetAllPostsAsync();

            ViewData["AuthorId"] = new SelectList(authors, "AuthorId", "FullName", tag?.AuthorId);
            ViewData["PostId"] = new SelectList(posts, "Id", "Id", tag?.PostId);
        }  
        #endregion

    }
}
