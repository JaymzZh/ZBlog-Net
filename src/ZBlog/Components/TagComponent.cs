using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZBlog.Models;

namespace ZBlog.Components
{
    [ViewComponent(Name = "Tag")]
    public class TagComponent : ViewComponent
    {
        public TagComponent(ZBlogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private ZBlogDbContext DbContext { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await DbContext.Tags.Include(c => c.PostTags).ToListAsync();

            return View(tags);
        }
    }
}