using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    DbContext.Posts
                        .OrderByDescending(p => p.Visits)
                        .Take(10)
                        .ToListAsync();

            return View(posts);
        }
    }
}