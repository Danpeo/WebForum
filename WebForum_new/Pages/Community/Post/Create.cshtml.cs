using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Extensions;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels.Post;

namespace WebForum_new.Pages.Community.Post;

[Authorize]
public class CreateModel : PageModel
{
    [BindProperty] public CreatePostViewModel PostVM { get; set; } = new();

    private readonly IPostService _postService;
    private readonly ICommunityService _communityService;
    private UserManager<AppUser> _userManager;
    
    public CreateModel(IPostService postService, ICommunityService communityService, ILogger<CreateModel> logger,
        UserManager<AppUser> userManager)
    {
        _postService = postService;
        _communityService = communityService;
        _userManager = userManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(int id)
    {
        Models.Community? community = await _communityService.GetByIdAsync(id);
        AppUser? user = await _userManager.GetUserAsync(User);

        bool created = await _postService.CreateAsync(community, PostVM, user);
        
        return created ? this.RedirectBack() : LocalRedirect(Url.Content("~/"));
    }
}