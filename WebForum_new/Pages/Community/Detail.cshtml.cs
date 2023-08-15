using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Services;

namespace WebForum_new.Pages.Community;

public class DetailModel : PageModel
{
    public Models.Community? Community { get; set; } = new();
    public bool CanManageCommunity { get; set; }
    
    private readonly ICommunityService _communityService;
    private readonly IAuthorizationService _authService;

    public DetailModel(ICommunityService communityService, IAuthorizationService authService)
    {
        _communityService = communityService;
        _authService = authService;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        Community = await _communityService.GetByIdAsync(id);
        AuthorizationResult isAuthorised = await _authService
            .AuthorizeAsync(User, Community, "CanManageCommunity");

        CanManageCommunity = isAuthorised.Succeeded;
        
        if (Community == null)
            return NotFound();

        return Page();
    }
    
    
}