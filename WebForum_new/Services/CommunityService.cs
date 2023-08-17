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
                CreatedBy = c.CreatedBy
            })
            .ToListAsync();

        return communities;
    }

    public async Task<Community?> GetByIdAsync(int id) =>
        await Context.Communities
            .Include(c => c.Posts)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> CreateAsync(CreateCommunityViewModel? communityVM, AppUser? createdBy)
    {
        var newCommunity = new Community()
        {
            Name = communityVM.Name,
            Description = communityVM.Description,
            DateTimeCreated = DateTime.Now,
            CreatedBy = createdBy
        };
        //createdBy.Communities.Add(newCommunity);
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
}