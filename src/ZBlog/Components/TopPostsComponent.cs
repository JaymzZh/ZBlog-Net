﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using ZBlog.Models;

namespace ZBlog.Components
{
    [ViewComponent(Name = "TopPosts")]
    public class TopPostsComponent : ViewComponent
    {
        public TopPostsComponent(ZBlogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private ZBlogDbContext DbContext { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts =
                await
                    DbContext.Posts.Include(p => p.Catalog)
                        .Include(p => p.PostTags)
                        .ThenInclude(p => p.Tag)
                        .Include(p => p.User)
                        .OrderByDescending(p => p.Visits)
                        .Take(10)
                        .ToListAsync();
            
            return View(posts);
        }
    }
}