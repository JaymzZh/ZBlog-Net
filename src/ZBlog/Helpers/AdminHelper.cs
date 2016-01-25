using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class AdminHelper
    {
        public static bool IsAdmin(this IHtmlHelper self)
        {
            var result = self.ViewContext.HttpContext.Session.GetString("Admin")?.Equals("true", StringComparison.OrdinalIgnoreCase);
            return result ?? false;
        }
    }
}
