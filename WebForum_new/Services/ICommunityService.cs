using WebForum_new.Models;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Services;

public interface ICommunityService
{
    Task<bool> CreateAsync(CreateCommunityViewModel? communityVM, AppUser? createdBy);
    Task<List<ViewCommunityViewModel?>> GetAllAsync();
    Task<Community?> GetByIdAsync(int id);
    Task<bool> PermanentDeleteAsync(int id);
    Task<bool> SubscribeAsync(int communityId, AppUser user);
    Task<bool> UnsubscribeAsync(int communityId, AppUser user);
    Task<List<ViewCommunityViewModel>> GetSubscribedCommunitiesAsync(List<int> subscribedCommunityIds);
    Task<List<int>> GetSubscribedCommunityIdsAsync(AppUser currentUser);
    Task<List<AppUser>> GetSubscribersAsync(int communityId);
    Task<List<ViewCommunityViewModel>> SearchAsync(string searchQuery, DateTime? searchDate);
}