using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Http;

namespace ZBlog.Filters
{
    public class AdminRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsAdmin(context))
                context.Result = new RedirectResult("/Admin/Login?returnUrl=" + context.HttpContext.Request.Path);
            else
                base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!IsAdmin(context))
                context.Result = new RedirectResult("/Admin/Login?returnUrl=" + context.HttpContext.Request.Path);
            return base.OnActionExecutionAsync(context, next);
        }

        private static bool IsAdmin(ActionExecutingContext context)
        {
            return context.HttpContext.Session.GetString("Admin")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
        }
    }
}