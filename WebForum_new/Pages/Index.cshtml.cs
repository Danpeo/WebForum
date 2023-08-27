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
    private const int PostsPageSize = 2;

    public int? Page { get; set; }
    
    public List<ViewCommunityViewModel> CommunityViewModels { get; set; } = new();
    public List<Post>? Posts { get; set; } = new();
    
    public PaginationInfo PaginationInfo { get; set; } // Добавьте это свойство

    public PaginatedList<Post> PagedList { get; set; }
    
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

            Page = id;

            int pageNumber = id ?? 2;

            IQueryable<Post> postsQuery = _postService.GetPostsQuery(); // Получите IQueryable<Post> из сервиса

            PagedList = await PaginatedList<Post>.CreateAsync(postsQuery, pageNumber, PostsPageSize);
            
            PaginationInfo = new PaginationInfo
            {
                TotalPages = PagedList.TotalPages,
                CurrentPage = PagedList.PageIndex
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