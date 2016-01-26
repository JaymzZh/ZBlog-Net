using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZBlog.Models;

namespace ZBlog.Components
{
    [ViewComponent(Name = "Catalog")]
    public class CatalogComponent : ViewComponent
    {
        public CatalogComponent(ZBlogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private ZBlogDbContext DbContext { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await DbContext.Catalogs.Include(c => c.Posts).ToListAsync();

            return View(posts);
        }
    }
}