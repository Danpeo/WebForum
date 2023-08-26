using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels.Community;
using X.PagedList;

namespace WebForum_new.Pages;

public class IndexModel : PageModel
{
    private const int PostsPageSize = 2;

    public List<ViewCommunityViewModel> CommunityViewModels { get; set; } = new();
    public List<Post>? Posts { get; set; } = new();
    public IPagedList<Post>? PagedList { get; set; }
    
    private readonly ILogger<IndexModel> _logger;
    private readonly IAccountService _accountService;
    private ICommunityService _communityService;
    private IPostService _postService;
    private UserManager<AppUser> _userManager;

    public IndexModel(ILogger<IndexModel> logger, IAccountService accountService, ICommunityService communityService,
        UserManager<AppUser> userManager, IPostService postService)
    {
        _logger = logger;
        _accountService = accountService;
        _communityService = communityService;
        _userManager = userManager;
        _postService = postService;
    }

    public async Task<PageResult> OnGet(int? page)
    {
        AppUser? user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            List<int> subscribedCommunitiedIds = await _communityService.GetSubscribedCommunityIdsAsync(user);
            CommunityViewModels = await _communityService.GetSubscribedCommunitiesAsync(subscribedCommunitiedIds);

            int pageNumber = page ?? 1;

            PagedList =
                await _postService.GetPostsFromSubscribedCommunitiesAsync(user, pageNumber,
                    PostsPageSize);

        }

        return Page();
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await _accountService.Logout();
        return RedirectToPage();
    }
}