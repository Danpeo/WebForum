using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Filters;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels;

namespace WebForum_new.Pages.UserProfile;

[ValidateModel]
public class EditModel : PageModel
{
    [BindProperty]
    public EditUserProfilViewModel EditUserProfileVM { get; set; } = new();
    
    private readonly IUserProfileService _profileService;

    public EditModel(UserManager<AppUser> userManager, IUserProfileService profileService)
    {
        _profileService = profileService;
    }
    
    public async Task<PageResult> OnGet()
    {
        AppUser? appUser = await _profileService.GetInfoForUserAsync(User);

        if (appUser != null) 
            EditUserProfileVM = new EditUserProfilViewModel(appUser);

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        bool edited = await _profileService.Edit(User, EditUserProfileVM);

        if (edited)
            return RedirectToPage("/UserProfile/Info");

        return Page();
    }
}