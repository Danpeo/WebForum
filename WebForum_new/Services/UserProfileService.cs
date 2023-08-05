using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;

namespace WebForum_new.Services;

public interface IUserProfileService
{
    Task<AppUser?> GetInfoForUserAsync(ClaimsPrincipal user);
}

public class UserProfileService : CommonService<ApplicationDbContext>, IUserProfileService
{
    private readonly UserManager<AppUser> _userManager;

    public UserProfileService(ApplicationDbContext context, UserManager<AppUser> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public async Task<AppUser?> GetInfoForUserAsync(ClaimsPrincipal user) => 
        await _userManager.GetUserAsync(user);
}