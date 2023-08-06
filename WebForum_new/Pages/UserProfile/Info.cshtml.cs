using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;

namespace WebForum_new.Pages.UserProfile;

[Authorize]
public class InfoModel : PageModel
{
    [BindProperty]
    public AppUser? AppUser { get; set; }
    
    private readonly IUserProfileService _profileService;

    public InfoModel(UserManager<AppUser> userManager, IUserProfileService profileService)
    {
        _profileService = profileService;
    }
    
    public async Task<IActionResult> OnGet()
    {
        AppUser = await _profileService.GetInfoForUserAsync(User);
        if (AppUser == null)
        {
            return NotFound();
        }
        
        return Page();
    }
}