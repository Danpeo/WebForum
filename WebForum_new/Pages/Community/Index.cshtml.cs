using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Pages.Community;

public class ViewModel : PageModel
{
    public List<ViewCommunityViewModel> CommunityViewModels { get; set; } = new();
    
    private ICommunityService _communityService;

    public ViewModel(ICommunityService communityService, UserManager<AppUser> userManager)
    {
        _communityService = communityService;
    }

    public async Task<PageResult> OnGet()
    {
        CommunityViewModels = await _communityService.GetAllAsync();

        return Page();
    }
}