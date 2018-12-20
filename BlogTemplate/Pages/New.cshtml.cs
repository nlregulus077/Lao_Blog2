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
using System.Threading.Tasks;

namespace Mom_Blog.Pages
{
    [Authorize]
    public class NewModel : PageModel
    {
        private readonly Mom_Blog.Data.ApplicationDbContext _context;
        private readonly SlugGenerator _slugGenerator;
        private readonly ExcerptGenerator _excerptGenerator;

        public NewModel(Mom_Blog.Data.ApplicationDbContext context, SlugGenerator slugGenerator, ExcerptGenerator excerptGenerator)
        {
            _context = context;
            _slugGenerator = slugGenerator;
            _excerptGenerator = excerptGenerator;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Mom_Blog.Models.Post Post { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyPost = new Post();

            if (await TryUpdateModelAsync<Post>(
                emptyPost,
                "post",
                p => p.Title, p => p.Body, p => p.Excerpt ))
            {
                _context.Post.Add(Post);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
           
        }
        //public NewPostViewModel NewPost { get; set; }

        //public void OnGet()
        //{           
        //}

        //[ValidateAntiForgeryToken]
        //public IActionResult OnPostPublish()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        SavePost(NewPost, true);
        //        return Redirect("/Index");
        //    }

        //    return Page();
        //}

        //[ValidateAntiForgeryToken]
        //public IActionResult OnPostSaveDraft()
        //{
        //    if(ModelState.IsValid)
        //    {
        //        SavePost(NewPost, false);
        //        return Redirect("/Drafts");
        //    }

        //    return Page();
        //}

        //private void SavePost(NewPostViewModel newPost, bool publishPost)
        //{
        //    if (string.IsNullOrEmpty(newPost.Excerpt))
        //    {
        //        newPost.Excerpt = _excerptGenerator.CreateExcerpt(newPost.Body);
        //    }

        //    Post post = new Post {
        //        Title = newPost.Title,
        //        Body = newPost.Body,
        //        Excerpt = newPost.Excerpt,
        //        Slug = _slugGenerator.CreateSlug(newPost.Title),
        //        LastModified = DateTimeOffset.Now,
        //        IsDeleted = false,
        //    };

        //    if (publishPost)
        //    {
        //        post.PubDate = DateTimeOffset.Now;
        //        post.IsPublic = true;
        //    }

        //    if (Request != null)
        //    {
        //        _dataStore.SaveFiles(Request.Form.Files.ToList());
        //    }

        //    _dataStore.SavePost(post);
        //}

        public class NewPostViewModel
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Body { get; set; }
            public string Excerpt { get; set; }
        }
    }
}
