using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZBlog.Models;

namespace ZBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ZBlogDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public HomeController(ZBlogDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        [Route("{page:int?}")]
        public async Task<IActionResult> Index(int? page = 1)
        {
            var posts = _dbContext.Posts.Include(p => p.Catalog)
                .Include(p => p.PostTags)
                .ThenInclude(p => p.Tag)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreateTime);

            var result = await GetPagedResult(page, posts);

            ViewData["Title"] = "Home";

            return View(result);
        }

        [Route("Catalog/{title}/{page:int?}")]
        public async Task<IActionResult> Catalog(string title, int? page = 1)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest();

            var posts = _dbContext.Posts.Include(p => p.Catalog)
            .Include(p => p.PostTags)
            .ThenInclude(p => p.Tag)
            .Include(p => p.User)
            .Where(p => p.Catalog.Title.Equals(title))
            .OrderByDescending(p => p.CreateTime);

            var result = await GetPagedResult(page, posts);

            ViewData["Title"] = title;

            return View("Index" , result);
        }

        [Route("Tag/{name}/{page:int?}")]
        public async Task<IActionResult> Tag(string name, int? page = 1)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest();

            var posts = _dbContext.Posts.Include(p => p.Catalog)
            .Include(p => p.PostTags)
            .ThenInclude(p => p.Tag)
            .Include(p => p.User)
            .Where(p => p.PostTags.Any(t => t.Tag.Name.Equals(name)))
            .OrderByDescending(p => p.CreateTime);

            var result = await GetPagedResult(page, posts);

            ViewData["Title"] = name;

            return View("Index" , result);
        }

        [Route("User/{name}/{page:int?}")]
        public async Task<IActionResult> Users(string name, int? page = 1)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest();

            var posts = _dbContext.Posts.Include(p => p.Catalog)
            .Include(p => p.PostTags)
            .ThenInclude(p => p.Tag)
            .Include(p => p.User)
            .Where(p => p.User.NickName.Equals(name))
            .OrderByDescending(p => p.CreateTime);

            var result = await GetPagedResult(page, posts);

            ViewData["Title"] = name;

            return View("Index" , result);
        }

        private async Task<List<Post>> GetPagedResult(int? page, IOrderedQueryable<Post> posts)
        {
            List<Post> result;
            int? pagePrev = null, pageNext = null;
            if (page.HasValue && page.Value > 1)
            {
                int p = page.Value;
                result =
                    await posts.Skip((p - 1) * 10)
                        .Take(10)
                        .ToListAsync();
                pagePrev = p - 1;
                pageNext = p + 1;
            }
            else
            {
                result = await posts.Take(10).ToListAsync();
            }

            if (pagePrev <= 0)
            {
                pagePrev = null;
            }
            var postCount = posts.Count();
            int pageCount = postCount / 10;
            pageCount += postCount % 10 == 0 ? 0 : 1;
            if (pageCount < pageNext)
            {
                pageNext = null;
            }

            ViewBag.PagePrev = pagePrev;
            ViewBag.PageNext = pageNext;

            return result;
        }

        public IActionResult About()
        {
            ViewData["About"] = _appSettings != null
                   ? _dbContext.Users.FirstOrDefault(u => u.Name.Equals(_appSettings.UserInfo.Name))?.About ??
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
