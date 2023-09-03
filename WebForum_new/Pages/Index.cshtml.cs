using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Pagination;
using WebForum_new.Services;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Pages;

public class IndexModel : PageModel
{
    private const int PostsPageSize = 5;
    public List<ViewCommunityViewModel> CommunityViewModels { get; set; } = new();
    public PaginationInfo PaginationInfo { get; set; } = new();
    public PaginatedList<Post> PostPagedList { get; set; } = new();
    
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

    
    public async Task<IActionResult> OnGet(int? id)
    {
        AppUser? user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            List<int> subscribedCommunityIds = await _communityService.GetSubscribedCommunityIdsAsync(user);
            CommunityViewModels = await _communityService.GetSubscribedCommunitiesAsync(subscribedCommunityIds);
            
            
            List<Post> posts = await _postService.GetPostsFromSubscribedCommunitiesAsync(user);

            PostPagedList = PaginatedList<Post>.Create(posts, id ?? 1, PostsPageSize);
            
            PaginationInfo = new PaginationInfo
            {
                TotalPages = PostPagedList.TotalPages,
                CurrentPage = PostPagedList.PageIndex
            };
        }

        return Page();
    }


    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await _accountService.Logout();
        return RedirectToPage();
    }
}