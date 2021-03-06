using System;
using System.Collections.Generic;
using System.Linq;
using Mom_Blog.Models;
using Mom_Blog.Pages;
using Mom_Blog.Tests.Fakes;
using Xunit;

namespace Mom_Blog.Tests.Pages
{
    public class DraftsTests
    {
        [Fact]
        public void GetDrafts_ShowDraftSummaries()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);

            Post draftPost = new Post
            {
                Title = "Draft Post",
                IsPublic = false,
                PubDate = DateTime.UtcNow,
            };
            Post publishedPost = new Post
            {
                Title = "Published Post",
                IsPublic = true,
                PubDate = DateTime.UtcNow,
            };

            testDataStore.SavePost(draftPost);
            testDataStore.SavePost(publishedPost);

            DraftsModel testDraftsModel = new DraftsModel(testDataStore);
            testDraftsModel.OnGet();
            DraftSummaryModel testDraftSummaryModel = testDraftsModel.DraftSummaries.First();

            Assert.Equal(1, testDraftsModel.DraftSummaries.Count());
            Assert.Equal(draftPost.Id, testDraftSummaryModel.Id);
            Assert.Equal("Draft Post", testDraftSummaryModel.Title);
            Assert.Equal(draftPost.PubDate, testDraftSummaryModel.PublishTime);
            Assert.Equal(0, testDraftSummaryModel.CommentCount);
        }
    }
}
