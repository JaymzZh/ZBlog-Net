using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using ZBlog.Common;
using ZBlog.Filters;
using ZBlog.Models;
using ZBlog.Services;
using ZBlog.ViewModels.Admin;

namespace ZBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly ZBlogDbContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public AdminController(
            ZBlogDbContext dbContext,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AdminController>();
        }

        //
        // GET: /Admin/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _dbContext.Users.Where(u => u.Email.Equals(model.Email) && u.Password.Equals(Util.GetMd5(model.Password))).FirstAsync();
                if (null != user)
                {
                    _logger.LogInformation(1, "User logged in.");
                    HttpContext.Session.SetString("Admin", "true");
                    HttpContext.Session.SetString("AdminName", user.Name);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Admin/Logout
        [AdminRequired]
        [HttpGet]
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // GET: /Admin/Index
        [AdminRequired]
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var user = await GetCurrentUserAsync();
            return View(user);
        }

        //
        // GET: /Admin/ChangePassword
        [AdminRequired]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Admin/ChangePassword
        [AdminRequired]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                if (!user.Password.Equals(Util.GetMd5(model.OldPassword)))
                {
                    ModelState.AddModelError(string.Empty, "Old password is not correct.");
                    return View(model);
                }
                user.Password = Util.GetMd5(model.NewPassword);
                var result = await _dbContext.SaveChangesAsync();
                if (result > 0)
                {
                    _logger.LogInformation(3, "Change password successfully.");
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                ModelState.AddModelError(string.Empty, "Unable to change the password, please try again later.");
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        #region Helpers

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
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
