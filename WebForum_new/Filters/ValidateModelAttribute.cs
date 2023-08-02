using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebForum_new.Filters;

[AttributeUsage(AttributeTargets.All)]
public class ValidateModelAttribute : Attribute, IPageFilter
{
    public Func<PageHandlerExecutingContext, IActionResult> OnInvalidModelState { get; set; } =
        context => context.Result = new PageResult(); 
    
    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            OnInvalidModelState.Invoke(context);
        }
    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
    }
}