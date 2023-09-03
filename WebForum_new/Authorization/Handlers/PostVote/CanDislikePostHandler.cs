using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebForum_new.Authorization.Requirements.PostVote;
using WebForum_new.Data;
using WebForum_new.Models;

namespace WebForum_new.Authorization.Handlers.PostVote;

public class CanDislikePostHandler: AuthorizationHandler<CanDislikePostRequirement, Post>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CanDislikePostHandler(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CanDislikePostRequirement requirement,
        Post resource)
    {
        AppUser? appUser = await _userManager.GetUserAsync(context.User);

        if (appUser == null)
        {
            return;
        }

        bool dislikedPost = _context.PostVotes
            .Any(cs => cs.AppUser == appUser && cs.PostId == resource.Id && cs.VoteType == VoteType.Dislike);

        if (!dislikedPost)
            context.Succeed(requirement);
    }
}