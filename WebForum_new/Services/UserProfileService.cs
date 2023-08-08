using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels;

namespace WebForum_new.Services;

public class UserProfileService : CommonService<ApplicationDbContext>, IUserProfileService
{
    private readonly UserManager<AppUser> _userManager;

    public UserProfileService(ApplicationDbContext context, UserManager<AppUser> userManager) : base(context)
    {
        _userManager = userManager;
    }

    public async Task<AppUser?> GetInfoForUserAsync(ClaimsPrincipal user) => 
        await _userManager.GetUserAsync(user);

    public async Task<bool> Edit(ClaimsPrincipal user, EditUserProfilViewModel newUserInfo)
    {
        AppUser? userInfo = await GetInfoForUserAsync(user);

        if (userInfo != null) 
            userInfo.UserBio = newUserInfo.UserBio;

        return await SaveAsync();
    }
}