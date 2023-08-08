using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Services;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Pages.Community;

public class CreateModel : PageModel
{
    [BindProperty] public CreateCommunityViewModel CommunityVM { get; set; } = new();

    private readonly ICommunityService _communityService;

    public CreateModel(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            bool created = await _communityService.CreateAsync(CommunityVM);
            if (created)
                return LocalRedirect(Url.Content("~/"));
        }

        return Page();
    }
}