using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZBlog.Filters;
using ZBlog.Models;

namespace ZBlog.Controllers
{
    [AdminRequired]
    public class CatalogController : Controller
    {
        private readonly ZBlogDbContext _dbContext;
        private readonly ILogger _logger;
        
        public CatalogController(ZBlogDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<PostController>();
        }

        // GET: /Catalog
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                   message == ManageMessageId.CatalogNewSuccess ? "Your catalog has been posted."
                   : message == ManageMessageId.CatalogEditSuccess ? "Your catalog has been updated."
                   : message == ManageMessageId.CatalogDeleteSuccess ? "Your catalog has been deleted."
                   : message == ManageMessageId.Error ? "An error has occurred."
                   : "";

            var catalogs = await _dbContext.Catalogs.Include(c => c.Posts).ToListAsync();

            return View(catalogs);
        }

        // GET: /Catalog/New
        [Route("/Catalog/New")]
        public IActionResult New()
        {
            return View();
        }

        // Post: /Catalog/New
        [HttpPost]
        [Route("/Catalog/New")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(Catalog model, CancellationToken requestAborted)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(model);
                await _dbContext.SaveChangesAsync(requestAborted);

                _logger.LogDebug($"Added catalog<{model.Title}>");

                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.CatalogNewSuccess });
            }

            return View(model);
        }
        
        // GET: /Catalog/Edit/{id}
        [Route("/Catalog/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogDebug($"Edit catalog<{id}>");

            if (!id.HasValue || id <= 0)
                return BadRequest();

            var catalog = await _dbContext.Catalogs.FirstOrDefaultAsync(c => c.Id == id);

            if (catalog == null)
            {
                return NotFound();
            }

            ViewData["Title"] = $"Catalog Edit<{catalog.Title}>";

            return View(catalog);
        }

        // Post: /Catalog/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Catalog/Edit/{id}")]
        public async Task<IActionResult> Edit(Catalog model, CancellationToken requestAborted)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Update(model);
                await _dbContext.SaveChangesAsync(requestAborted);

                _logger.LogDebug($"Updated catalog<{model.Title}>");

                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.CatalogEditSuccess });
            }

            return View(model);
        }

        // GET: /Catalog/Delete/{id}
        [Route("/Catalog/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogDebug($"Delete catalog<{id}>");

            if (!id.HasValue || id <= 0)
                return BadRequest();

            var catalog = await _dbContext.Catalogs.FirstOrDefaultAsync(c => c.Id.Equals(id));
            if (catalog == null)
                return NotFound();

            ViewData["Title"] = $"Catalog<{catalog.Title}> Delete Confirmation";

            return View(catalog);
        }

        // POST: /Catalog/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Catalog/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id, CancellationToken requestAborted)
        {
            if (!id.HasValue || id <= 0)
                return BadRequest();

            var catalog = await _dbContext.Catalogs.FirstOrDefaultAsync(c => c.Id.Equals(id), requestAborted);
            _dbContext.Remove(catalog);
            await _dbContext.SaveChangesAsync(requestAborted);
            _logger.LogDebug($"Deleted catalog<{catalog.Title}>");

            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.CatalogDeleteSuccess });
        }

        #region Helpers

        public enum ManageMessageId
        {
            CatalogNewSuccess,
            CatalogEditSuccess,
            CatalogDeleteSuccess,
            Error
        }

        #endregion
    }
}

