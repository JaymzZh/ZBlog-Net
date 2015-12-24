using System.Linq;
using Microsoft.AspNet.Mvc;
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

        public IActionResult Index()
        {
            return View();
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
