using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebForum_new.Authorization.Requirements;
using WebForum_new.Data;
using WebForum_new.Models;

namespace WebForum_new.Authorization.Handlers;

public class IsCommunitySubscriberHandler : AuthorizationHandler<IsCommunitySubscriberRequirement, Community>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public IsCommunitySubscriberHandler(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IsCommunitySubscriberRequirement requirement, Community resource)
    {
        AppUser? appUser = await _userManager.GetUserAsync(context.User);
        
        if(appUser == null)
        {
            return;
        }

        bool isSubscriber = _context.CommunitySubscriptions
            .Any(cs => cs.AppUser == appUser && cs.CommunityId == resource.Id);
        
        if(isSubscriber) 
            context.Succeed(requirement);
    }
}