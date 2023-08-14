using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebForum_new.Authorization.Requirements;
using WebForum_new.Models;

namespace WebForum_new.Authorization.Handlers;

public class IsCommunityOwnerHandler : AuthorizationHandler<IsCommunityOwnerRequirement, Community>
{
    private readonly UserManager<AppUser> _userManager;

    public IsCommunityOwnerHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCommunityOwnerRequirement requirement,
        Community resource)
    {
        AppUser? appUser = await _userManager.GetUserAsync(context.User);
        
        if(appUser == null)
        {
            return;
        }

        if(resource.CreatedById == appUser.Id) 
            context.Succeed(requirement);
    }
}