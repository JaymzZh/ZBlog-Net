using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

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