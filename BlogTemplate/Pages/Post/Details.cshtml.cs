using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mom_Blog.Data;
using Mom_Blog.Models;

namespace Lao_MomBlog.Pages.Post
{
    public class DetailsModel : PageModel
    {
        private readonly Mom_Blog.Data.ApplicationDbContext _context;

        public DetailsModel(Mom_Blog.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Mom_Blog.Models.Post Post { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = await _context.Post.FirstOrDefaultAsync(m => m.Id == id);

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
