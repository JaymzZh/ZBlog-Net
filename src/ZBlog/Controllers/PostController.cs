using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using ZBlog.Filters;
using ZBlog.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ZBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly ZBlogDbContext _dbContext;
        private readonly ILogger _logger;

        public PostController(ZBlogDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<PostController>();
        }

        // GET: /Post/Index
        [AdminRequired]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.PostNewSuccess ? "Your post has been posted."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var posts =
                await
                    _dbContext.Posts.Include(p => p.Catalog)
                        .Include(p => p.PostTags)
                        .ThenInclude(p => p.Tag)
                        .Include(p => p.User)
                        .OrderByDescending(p => p.CreateTime)
                        .ToListAsync();

            return View(posts);
        }

        // GET: /Post/New
        [AdminRequired]
        public async Task<IActionResult> New()
        {
            var catalog = await _dbContext.Catalogs.OrderByDescending(c => c.PRI).ToListAsync();
            ViewBag.Catalogs = catalog;
            _logger.LogDebug("Catalog count: " + catalog.Count);
            return View();
        }

        // POST: /Post/New
        [HttpPost]
        [AdminRequired]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Post model, string tags)
        {
            if (ModelState.IsValid)
            {
                model.CreateTime = model.LastEditTime = DateTime.Now;
                model.Visits = 0;
                var length = model.Content.Length;
                model.Summary = model.Content.Substring(0, length >= 100 ? 100 : length);
                model.User = await GetCurrentUserAsync();
                _dbContext.Add(model);
                await _dbContext.SaveChangesAsync();
                if (!string.IsNullOrWhiteSpace(tags))
                {
                    _dbContext.RemoveRange(model.PostTags);
                    var tagArray = tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var t in tagArray)
                    {
                        var postTag = new PostTag
                        {
                            PostId = model.Id
                        };
                        var tag =
                            await _dbContext.Tags.FirstOrDefaultAsync(
                                x => x.Name.Equals(t, StringComparison.OrdinalIgnoreCase));
                        if (tag == null)
                        {
                            tag = new Tag
                            {
                                Name = t
                            };
                            _dbContext.Add(tag);
                            await _dbContext.SaveChangesAsync();
                        }
                        postTag.TagId = tag.Id;
                        _dbContext.Add(postTag);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.PostNewSuccess });
            }
            var catalog = await _dbContext.Catalogs.OrderByDescending(c => c.PRI).ToListAsync();
            ViewBag.Catalogs = catalog;
            return View(model);
        }
        
        #region Helpers

        public enum ManageMessageId
        {
            PostNewSuccess,
            Error
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.Session.GetString("AdminName")));
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
