using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using ZBlog.Models;

namespace ZBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ZBlogDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public HomeController(ZBlogDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["About"] = _configuration != null
                   ? _dbContext.Users.FirstOrDefault(u => u.Name.Equals(_configuration["User:Name"]))?.About ??
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
