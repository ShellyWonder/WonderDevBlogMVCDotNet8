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
    public class TagsController(ITagsService tagsService,
                                 IApplicationUserService applicationUserService,
                                  IPostService postService) : Controller
    {
        private readonly ITagsService _tagsService = tagsService;
        private readonly IApplicationUserService _applicationUserService = applicationUserService;
        private readonly IPostService _postService = postService;

        // GET: Tags
        public async Task<IActionResult> Index()
        {
            var tags = await _tagsService.GetAllTagsAsync();
            return View(tags);
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _tagsService.GetTagByIdAsync(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tags/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownLists();
            return View();
        }

        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,AuthorId,Text")] Tag tag)
        {
            if (ModelState.IsValid)
            {
               await _tagsService.AddTagAsync(tag);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownLists();
            return View(tag);
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _tagsService.GetTagByIdAsync(id.Value);

            if (tag == null)
            {
                return NotFound();
            }
            await PopulateDropDownLists();
            return View(tag);
        }

        // POST: Tags/Edit/5
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
                   
                    await _tagsService.UpdateTagAsync(tag);
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

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _tagsService.GetTagByIdAsync(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tagsService.DeleteTagAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return _tagsService.TagExists(id);
        }

        private async Task PopulateDropDownLists(Tag? tag = null)
        {
            //NOTE: CHANGE VAR DEFINITIONS WHEN ROLES ARE IMPLEMENTED
            var authors = await _applicationUserService.GetAllUsersAsync();
            var posts = await _postService.GetAllPostsAsync();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "Id", tag?.AuthorId);
            ViewData["PostId"] = new SelectList(posts, "Id", "Id", tag?.PostId);
        }
    }
}
