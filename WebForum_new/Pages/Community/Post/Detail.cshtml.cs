using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels.Comment;

namespace WebForum_new.Pages.Community.Post;

public class DetailModel : PageModel
{
    [BindProperty] public CreateCommentViewModel CommentVM { get; set; } = new();

    [BindProperty] public Models.Post? Post { get; set; } = new();

    public bool CanPutLike { get; set; }
    public bool CanPutDisike { get; set; }

    private readonly IAuthorizationService _authService;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private UserManager<AppUser> _userManager;

    public DetailModel(IPostService postService, ICommentService commentService, UserManager<AppUser> userManager,
        IAuthorizationService authService)
    {
        _postService = postService;
        _commentService = commentService;
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        Post = await _postService.GetByIdAsync(id);

        await CheckUserPermissions();

        if (Post == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostCreateComment(int id)
    {
        Post = await _postService.GetByIdAsync(id);
        AppUser? user = await _userManager.GetUserAsync(User);

        bool created = Post != null && await _commentService.CreateAsync(Post, CommentVM, user);
        if (created)
            return LocalRedirect(Url.Content("~/"));

        return Page();
    }

    public async Task<IActionResult> OnPostVote(int id, VoteType voteType)
    {
        Post = await _postService.GetByIdAsync(id);
        AppUser? user = await _userManager.GetUserAsync(User);

        bool voted = user != null && await _postService.AddVoteAsync(id, user, voteType);

        if (voted)
            return LocalRedirect(Url.Content("~/"));

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveVote(int id, VoteType voteType)
    {
        Post = await _postService.GetByIdAsync(id);
        AppUser? user = await _userManager.GetUserAsync(User);

        bool removedVote = user != null && await _postService.RemoveVoteAsync(id, user, voteType);

        if (removedVote)
            return LocalRedirect(Url.Content("~/"));
        
        return Page();
    }

    private async Task CheckUserPermissions()
    {
        CanPutLike = (await _authService
            .AuthorizeAsync(User, Post, "CanLikePost")).Succeeded;
        
        CanPutDisike = (await _authService
            .AuthorizeAsync(User, Post, "CanDislikePost")).Succeeded;
    }
}