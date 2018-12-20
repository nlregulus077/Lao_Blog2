using System;
using System.Collections.Generic;
using System.Linq;
using Mom_Blog.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mom_Blog.Pages
{

    public class IndexModel : PageModel
    {
        private readonly Mom_Blog.Data.ApplicationDbContext _context;

        public IndexModel(Mom_Blog.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public string TitleSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Post> Post { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            IQueryable<Post> postIQ = from p in _context.Post
                                      select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                postIQ = postIQ.Where(p => p.Title.Contains(searchString)
                                        || p.Body.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    postIQ = postIQ.OrderByDescending(p => p.Title);
                    break;

                case "Date":
                    postIQ = postIQ.OrderBy(p => p.PubDate);
                    break;

                case "date_desc":
                    postIQ = postIQ.OrderByDescending(p => p.Title);
                    break;
            }

            int pageSize = 6;
            Post = await PaginatedList<Post>.CreateAsync(
                postIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        //    const string StorageFolder = "BlogFiles";

        //    private readonly BlogDataStore _dataStore;

        //    public IEnumerable<PostSummaryModel> PostSummaries { get; private set; }

        //    public IndexModel(BlogDataStore dataStore)
        //    {
        //        _dataStore = dataStore;
        //    }

        //    public void OnGet()
        //    {
        //        Func<Post, bool> postFilter = p => p.IsPublic;
        //        Func<Post, bool> deletedPostFilter = p => !p.IsDeleted;
        //        IEnumerable<Post> postModels = _dataStore.GetAllPosts().Where(postFilter).Where(deletedPostFilter);

        //        PostSummaries = postModels.Select(p => new PostSummaryModel {
        //            Id = p.Id,
        //            Slug = p.Slug,
        //            Title = p.Title,
        //            Excerpt = p.Excerpt,
        //            PublishTime = p.PubDate,
        //            CommentCount = p.Comments.Where(c => c.IsPublic).Count(),
        //        });
        //    }

        //    public class PostSummaryModel
        //    {
        //        public Guid Id { get; set; }
        //        public string Slug { get; set; }
        //        public string Title { get; set; }
        //        public DateTimeOffset PublishTime { get; set; }
        //        public string Excerpt { get; set; }
        //        public int CommentCount { get; set; }
        //   }
    }
}
