using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels;
using WebForum_new.ViewModels.Community;

namespace WebForum_new.Services;

public interface ICommunityService
{
    Task<bool> CreateAsync(CreateCommunityViewModel? communityVM);
    Task<List<ViewCommunityViewModel>> GetAllAsync();
}

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
                Name = c.Name,
                Description = c.Description,
                DateTimeCreated = c.DateTimeCreated,
                Posts = c.Posts
            })
            .ToListAsync();

        return communities;
    }
    
    public async Task<bool> CreateAsync(CreateCommunityViewModel? communityVM)
    {
        var newCommunity = new Community()
        {
            Name = communityVM.Name,
            Description = communityVM.Description,
            DateTimeCreated = DateTime.Now,
        };
        Context.Communities.Add(newCommunity);
        
        return await SaveAsync();
    }
}