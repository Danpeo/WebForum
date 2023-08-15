using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebForum_new.Models;

namespace WebForum_new.TagHelpers;

[HtmlTargetElement("for-not-logged")]
public class ForNotLoggedTagHelper : TagHelper
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly SignInManager<AppUser> _signInManager;

    public ForNotLoggedTagHelper(IHttpContextAccessor contextAccessor, SignInManager<AppUser> signInManager)
    {
        _contextAccessor = contextAccessor;
        _signInManager = signInManager;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (_contextAccessor.HttpContext != null && _signInManager.IsSignedIn(_contextAccessor.HttpContext.User))
        {
            output.SuppressOutput();
        }
        
    }
}