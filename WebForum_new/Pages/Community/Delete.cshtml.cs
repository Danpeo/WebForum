using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Services;

namespace WebForum_new.Pages.Community;

[Authorize]
public class DeleteModel : PageModel
{
    [BindProperty] 
    public Models.Community? Community { get; set; } = new();

    private readonly IAuthorizationService _authService;

    private readonly ICommunityService _communityService;

    public DeleteModel(ICommunityService communityService, IAuthorizationService authService)
    {
        _communityService = communityService;
        _authService = authService;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        Community = await _communityService.GetByIdAsync(id);

        if (Community == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        Community = await _communityService.GetByIdAsync(id);
        AuthorizationResult authResult = await _authService.AuthorizeAsync(User, Community, "CanManageCommunity");

        if (!authResult.Succeeded)
        {
            return new ForbidResult();
        }

        bool deleted = await _communityService.PermanentDeleteAsync(id);
        if (deleted)
            return LocalRedirect(Url.Content("~/"));

        return Page();
    }
}