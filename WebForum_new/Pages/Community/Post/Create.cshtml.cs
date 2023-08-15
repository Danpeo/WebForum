using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Services;
using WebForum_new.ViewModels.Post;

namespace WebForum_new.Pages.Community.Post;

[Authorize]
public class CreateModel : PageModel
{
    [BindProperty] public CreatePostViewModel PostVM { get; set; } = new();
    
    private readonly IPostService _postService;
    private readonly ICommunityService _communityService;


    public CreateModel(IPostService postService, ICommunityService communityService, ILogger<CreateModel> logger)
    {
        _postService = postService;
        _communityService = communityService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(int id)
    {
        Models.Community? community = await _communityService.GetByIdAsync(id);
        
        bool created = await _postService.CreateAsync(community, PostVM);
        if (created)
            return LocalRedirect(Url.Content("~/"));

        return Page();
    }
}