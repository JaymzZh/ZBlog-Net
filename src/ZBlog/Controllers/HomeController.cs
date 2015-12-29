using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using ZBlog.Models;

namespace ZBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ZBlogDbContext _dbContext;

        public HomeController(ZBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("{page:int?}")]
        public async Task<IActionResult> Index(int? page)
        {
            var posts = _dbContext.Posts.Include(p => p.Catalog)
                .Include(p => p.PostTags)
                .ThenInclude(p => p.Tag)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreateTime);

            List<Post> result;
            if (page.HasValue)
            {
                result =
                    await posts.Skip(page.Value * 10)
                    .Take(10)
                    .ToListAsync();
            }
            else
            {
                result = await posts.Take(10).ToListAsync();
            }

            return View(result);
        }

        [Route("Catalog/{title}/{page:int?}")]
        public async Task<IActionResult> Catalog(string title, int? page)
        {
            if (string.IsNullOrWhiteSpace(title))
                return HttpNotFound();

            var posts = _dbContext.Posts.Include(p => p.Catalog)
            .Include(p => p.PostTags)
            .ThenInclude(p => p.Tag)
            .Include(p => p.User)
            .Where(p => p.Catalog.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(p => p.CreateTime);

            List<Post> result;
            if (page.HasValue)
            {
                result =
                    await posts.Skip(page.Value * 10)
                    .Take(10)
                    .ToListAsync();
            }
            else
            {
                result = await posts.Take(10).ToListAsync();
            }

            return View("Index" , result);
        }

        [Route("Tag/{name}/{page:int?}")]
        public async Task<IActionResult> Tag(string name, int? page)
        {
            if (string.IsNullOrWhiteSpace(name))
                return HttpNotFound();

            var posts = _dbContext.Posts.Include(p => p.Catalog)
            .Include(p => p.PostTags)
            .ThenInclude(p => p.Tag)
            .Include(p => p.User)
            .Where(p => p.PostTags.Any(t => t.Tag.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(p => p.CreateTime);

            List<Post> result;
            if (page.HasValue)
            {
                result =
                    await posts.Skip(page.Value * 10)
                    .Take(10)
                    .ToListAsync();
            }
            else
            {
                result = await posts.Take(10).ToListAsync();
            }

            return View("Index" , result);
        }

        public IActionResult About([FromServices] IConfiguration configuration)
        {
            ViewData["About"] = configuration != null
                   ? _dbContext.Users.FirstOrDefault(u => u.Name.Equals(configuration["User:Name"]))?.About ??
                     "Nothing here..."
                   : "Can't find the configuration.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
