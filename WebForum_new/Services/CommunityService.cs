using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Services;

public class CommunityService : CommonService<ApplicationDbContext>, ICommunityService
{
    public CommunityService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<ViewCommunityViewModel>> GetAllAsync()
    {
        List<ViewCommunityViewModel> communities = await Context.Communities
            .Include(c => c.Posts)
            .Select(c => new ViewCommunityViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DateTimeCreated = c.DateTimeCreated,
                Posts = c.Posts,
                CreatedBy = c.AppUser
            })
            .ToListAsync();

        return communities;
    }

    public async Task<Community?> GetByIdAsync(int id) =>
        await Context.Communities
            .Include(c => c.Posts)
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> CreateAsync(CreateCommunityViewModel? communityVM, AppUser? createdBy)
    {
        var newCommunity = new Community()
        {
            Name = communityVM.Name,
            Description = communityVM.Description,
            DateTimeCreated = DateTime.Now,
            AppUser = createdBy
        };
        Context.Communities.Add(newCommunity);

        return await SaveAsync();
    }

    public async Task<bool> PermanentDeleteAsync(int id)
    {
        Community? communityToDelete = Context.Communities
            .FirstOrDefault(c => c != null && c.Id == id);

        if (communityToDelete != null)
        {
            Context.Communities.Remove(communityToDelete);
            return await SaveAsync();
        }

        return false;
    }

    public async Task<List<int>> GetSubscribedCommunityIdsAsync(AppUser currentUser)
    {
        List<int> subscribedCommunityIds = await Context.CommunitySubscriptions
            .Where(cs => cs.AppUser == currentUser)
            .Select(cs => cs.CommunityId)
            .ToListAsync();

        return subscribedCommunityIds;
    }

    public async Task<List<ViewCommunityViewModel>> GetSubscribedCommunitiesAsync(List<int> subscribedCommunityIds)
    {
        List<ViewCommunityViewModel> subscribedCommunities = await Context.Communities
            .Where(c => subscribedCommunityIds.Contains(c.Id))
            .Include(c => c.Posts)
            .Select(c => new ViewCommunityViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DateTimeCreated = c.DateTimeCreated,
                Posts = c.Posts,
                CreatedBy = c.AppUser
            })
            .ToListAsync();

        return subscribedCommunities;
    }

    public async Task<List<AppUser>> GetSubscribersAsync(int communityId)
    {
        List<AppUser> subscribers = await Context.CommunitySubscriptions
            .Where(cs => cs.CommunityId == communityId)
            .Select(cs => cs.AppUser)
            .ToListAsync();

        return subscribers;
    }

    public async Task<bool> SubscribeAsync(int communityId, AppUser subscriber)
    {
        var subscription = new CommunitySubscription()
        {
            CommunityId = communityId,
            AppUser = subscriber
        };

        subscriber.CommunitySubscriptions?.Add(subscription);
        Context.CommunitySubscriptions.Add(subscription);
        return await SaveAsync();
    }
}