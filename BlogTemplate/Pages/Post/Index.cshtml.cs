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
    public class IndexModel : PageModel
    {
        private readonly Mom_Blog.Data.ApplicationDbContext _context;

        public IndexModel(Mom_Blog.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Mom_Blog.Models.Post> Post { get;set; }

        public async Task OnGetAsync()
        {
            Post = await _context.Post.ToListAsync();
        }
    }
}
