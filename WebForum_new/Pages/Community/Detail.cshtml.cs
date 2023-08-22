using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;

namespace WebForum_new.Pages.Community;

public class DetailModel : PageModel
{
    public Models.Community? Community { get; set; } = new();
    public List<AppUser> Subscribers { get; set; } = new();
    public bool CanManageCommunity { get; set; }
    public bool IsSubscriber { get; set; }

    private readonly ICommunityService _communityService;
    private readonly IAuthorizationService _authService;
    private UserManager<AppUser> _userManager;


    public DetailModel(ICommunityService communityService, IAuthorizationService authService,
        UserManager<AppUser> userManager)
    {
        _communityService = communityService;
        _authService = authService;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        Community = await _communityService.GetByIdAsync(id);

        await CheckUserPermissions();

        if (Community == null)
            return NotFound();

        Subscribers = await _communityService.GetSubscribersAsync(id);

        return Page();
    }

    public async Task<IActionResult> OnPostSubscribe(int id)
    {
        Community = await _communityService.GetByIdAsync(id);
        AppUser? user = await _userManager.GetUserAsync(User);

        bool subscribed = Community != null && await _communityService.SubscribeAsync(Community.Id, user);

        if (subscribed)
            return LocalRedirect(Url.Content("~/"));

        return Page();
    }

    public async Task<IActionResult> OnPostUnsubscribe(int id)
    {
        Community = await _communityService.GetByIdAsync(id);
        AppUser? user = await _userManager.GetUserAsync(User);

        bool unsubscribed = Community != null && await _communityService.UnsubscribeAsync(Community.Id, user);

        if (unsubscribed)
            return LocalRedirect(Url.Content("~/"));

        return Page();
    }

    
    private async Task CheckUserPermissions()
    {
        CanManageCommunity = (await _authService
            .AuthorizeAsync(User, Community, "CanManageCommunity")).Succeeded;

        IsSubscriber = (await _authService
            .AuthorizeAsync(User, Community, "CommunitySubscriber")).Succeeded;
    }
}