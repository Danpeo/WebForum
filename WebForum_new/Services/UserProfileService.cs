using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels;

namespace WebForum_new.Services;

public interface IUserProfileService
{
    Task<AppUser?> GetInfoByIdAsync(int id);
}

public class UserProfileService : CommonService<ApplicationDbContext>, IUserProfileService
{
    public UserProfileService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<AppUser?> GetInfoByIdAsync(int id)
    {
        return await Context.AppUsers
            .Include(i => i.Communities)
            .Include(i => i.Posts)
            .FirstOrDefaultAsync(u => u.Id == id.ToString());
    }
}