using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Pagination;
using WebForum_new.Services;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Pages.Community;

public class ViewModel : PageModel
{
    private const int CommunitiesPageSize = 5;
    public PaginationInfo PaginationInfo { get; set; } = new();
    public PaginatedList<ViewCommunityViewModel> CommunityPagedList { get; set; } = new();

    public List<ViewCommunityViewModel?> CommunityViewModels { get; set; } = new();

    private ICommunityService _communityService;

    public ViewModel(ICommunityService communityService, UserManager<AppUser> userManager)
    {
        _communityService = communityService;
    }

    public async Task<PageResult> OnGet(int? id, string searchQuery, DateTime? searchDate)
    {
        if (string.IsNullOrEmpty(searchQuery) && !searchDate.HasValue)
            CommunityViewModels = await _communityService.GetAllAsync();
        else
            CommunityViewModels = await _communityService.SearchAsync(searchQuery, searchDate);

        CommunityPagedList =
            PaginatedList<ViewCommunityViewModel>.Create(CommunityViewModels, id ?? 1, CommunitiesPageSize);

        PaginationInfo = new PaginationInfo()
        {
            TotalPages = CommunityPagedList.TotalPages,
            CurrentPage = CommunityPagedList.PageIndex,
            RouteName = nameof(id)
        };

        return Page();
    }
}