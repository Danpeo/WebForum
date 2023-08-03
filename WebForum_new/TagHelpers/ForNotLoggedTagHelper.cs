using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebForum_new.TagHelpers;

[HtmlTargetElement("for-not-logged")]
public class ForNotLoggedTagHelper : TagHelper
{
    private readonly IHttpContextAccessor _contextAccessor;

    public ForNotLoggedTagHelper(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            output.SuppressOutput();
        }
        
    }
}