using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebForum_new.Extensions;

public static class PageModelExtensions
{
    public static IActionResult RedirectBack(this PageModel pageModel)
    {
        string refererUrl = pageModel.Request.Headers["Referer"].ToString();

        return new RedirectResult(refererUrl);
    }
}