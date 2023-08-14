using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Pages.Community;

[Authorize]
public class CreateModel : PageModel
{
    [BindProperty] 
    public CreateCommunityViewModel CommunityVM { get; set; } = new();

    private readonly ICommunityService _communityService;
    private readonly UserManager<AppUser> _userManager;

    public CreateModel(ICommunityService communityService, UserManager<AppUser> userManager)
    {
        _communityService = communityService;
        _userManager = userManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            AppUser? user = await _userManager.GetUserAsync(User);
            bool created = await _communityService.CreateAsync(CommunityVM, user);
            if (created)
                return LocalRedirect(Url.Content("~/"));
        }

        return Page();
    }
}