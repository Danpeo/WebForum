using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Services;

public class CommunityService : CommonService<ApplicationDbContext>, ICommunityService
{
    private readonly IImageService _imageService;

    public CommunityService(ApplicationDbContext context, IImageService imageService) : base(context)
    {
        _imageService = imageService;
    }

    public async Task<List<ViewCommunityViewModel>> GetAllAsync()
    {
        List<ViewCommunityViewModel> communities = await Context.Communities
            .Include(c => c.Posts)
            .Select(c => new ViewCommunityViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
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
        ImageUploadResult? image = await _imageService.UploadImageAsync(communityVM?.Image);
        
        var newCommunity = new Community()
        {
            Name = communityVM.Name,
            Description = communityVM.Description,
            Image = image == null ? string.Empty : image.Url.ToString(),
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

    public async Task<bool> SubscribeAsync(int communityId, AppUser user)
    {
        var subscription = new CommunitySubscription()
        {
            CommunityId = communityId,
            AppUser = user
        };

        user.CommunitySubscriptions?.Add(subscription);
        Context.CommunitySubscriptions.Add(subscription);
        return await SaveAsync();
    }

    public async Task<bool> UnsubscribeAsync(int communityId, AppUser user)
    {
        CommunitySubscription? subscription = await Context.CommunitySubscriptions
            .FirstOrDefaultAsync(sub => sub.CommunityId == communityId && sub.AppUser == user);

        if (subscription != null)
        {
            user.CommunitySubscriptions?.Remove(subscription);
            Context.CommunitySubscriptions.Remove(subscription);
        }

        return await SaveAsync();
    }

    public async Task<List<ViewCommunityViewModel>> SearchAsync(string searchQuery, DateTime? searchDate)
    {
        if (string.IsNullOrEmpty(searchQuery) && !searchDate.HasValue)
            return await GetAllAsync();

        IQueryable<Community?> query = Context.Communities
            .Where(c => (EF.Functions.Like(c.Name, $"%{searchQuery}%")) ||
                        EF.Functions.Like(c.Description, $"%{searchQuery}%"));

        if (searchDate.HasValue)
        {
            query = query.Where(community => community != null && community.DateTimeCreated.Date == searchDate.Value.Date);
        }
        
        IQueryable<ViewCommunityViewModel> result = query
            .Select(q => new ViewCommunityViewModel()
            {
                Id = q.Id,
                Name = q.Name,
                Description = q.Description,
                DateTimeCreated = q.DateTimeCreated,
                Posts = q.Posts,
                CreatedBy = q.AppUser                
            });
        
        return await result.ToListAsync();
    }
}