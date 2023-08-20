using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Pages.Community;

public class IndexSubscribedModel : PageModel
{
    public List<ViewCommunityViewModel> CommunityViewModels { get; set; } = new();

    private ICommunityService _communityService;
    private UserManager<AppUser> _userManager;

    public IndexSubscribedModel(ICommunityService communityService, UserManager<AppUser> userManager)
    {
        _communityService = communityService;
        _userManager = userManager;
    }

    public async Task<PageResult> OnGet()
    {
        AppUser? user = await _userManager.GetUserAsync(User);
        List<int> subscribedCommunitiedIds = await _communityService.GetSubscribedCommunityIdsAsync(user);
        CommunityViewModels = await _communityService.GetSubscribedCommunitiesAsync(subscribedCommunitiedIds);

        return Page();
    }
}