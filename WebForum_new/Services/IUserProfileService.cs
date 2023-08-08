using System.Security.Claims;
using WebForum_new.Models;
using WebForum_new.ViewModels;

namespace WebForum_new.Services;

public interface IUserProfileService
{
    Task<AppUser?> GetInfoForUserAsync(ClaimsPrincipal user);
    Task<bool> Edit(ClaimsPrincipal user, EditUserProfilViewModel newUserInfo);
}