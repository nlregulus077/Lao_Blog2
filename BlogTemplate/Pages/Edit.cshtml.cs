using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Mom_Blog.Models;
using Mom_Blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Mom_Blog.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mom_Blog.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Mom_Blog.Data.ApplicationDbContext _context;
        private readonly SlugGenerator _slugGenerator;
        private readonly ExcerptGenerator _excerptGenerator;

        public EditModel(Mom_Blog.Data.ApplicationDbContext context, SlugGenerator slugGenerator, ExcerptGenerator excerptGenerator)
        {
            _context = context;
            _slugGenerator = slugGenerator;
            _excerptGenerator = excerptGenerator;
        }

        [BindProperty]
        public Mom_Blog.Models.Post Post { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = await _context.Post.FindAsync(id);

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var postToUpdate = await _context.Post.FindAsync(id);

            if (await TryUpdateModelAsync<Post>(
                postToUpdate,
                "post",
                 p => p.Title, p => p.Body, p => p.Excerpt))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool PostExists(Guid id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }

    //[BindProperty]
    //public EditedPostModel EditedPost { get; set; }
    //public bool hasSlug { get; set; }

    //public void OnGet([FromRoute] string id)
    //{
    //    Post post = _dataStore.GetPost(id);

    //    if (post == null)
    //    {
    //        RedirectToPage("/Index");
    //    }

    //    EditedPost = new EditedPostModel
    //    {
    //        Title = post.Title,
    //        Body = post.Body,
    //        Excerpt = post.Excerpt,
    //    };

    //    hasSlug = !string.IsNullOrEmpty(post.Slug);

    //    ViewData["Slug"] = post.Slug;
    //    ViewData["id"] = post.Id;
    //}

    //[ValidateAntiForgeryToken]
    //public IActionResult OnPostPublish([FromRoute] string id, [FromForm] bool updateSlug)
    //{
    //    Post post = _dataStore.GetPost(id);
    //    if (ModelState.IsValid)
    //    {
    //        bool wasPublic = post.IsPublic;
    //        post.IsPublic = true;
    //        if (post.PubDate.Equals(default(DateTimeOffset)))
    //        {
    //            post.PubDate = DateTimeOffset.Now;
    //        }
    //        if (!hasSlug)
    //        {
    //            post.Slug = _slugGenerator.CreateSlug(post.Title);
    //        }
    //        UpdatePost(post, updateSlug, wasPublic);


    public class EditedPostModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public string Excerpt { get; set; }
    }
}
