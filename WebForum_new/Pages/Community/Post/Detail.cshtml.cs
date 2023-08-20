using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels.Comment;

namespace WebForum_new.Pages.Community.Post;

public class DetailModel : PageModel
{
    [BindProperty] 
    public CreateCommentViewModel CommentVM { get; set; } = new();

    [BindProperty]
    public Models.Post? Post { get; set; } = new();
    
    
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private UserManager<AppUser> _userManager;

    public DetailModel(IPostService postService, ICommentService commentService, UserManager<AppUser> userManager)
    {
        _postService = postService;
        _commentService = commentService;
        _userManager = userManager;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        Post = await _postService.GetByIdAsync(id);

        if (Post == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        Post = await _postService.GetByIdAsync(id);
        AppUser? user = await _userManager.GetUserAsync(User);

        bool created = Post != null && await _commentService.CreateAsync(Post, CommentVM, user);
        if (created)
            return LocalRedirect(Url.Content("~/"));

        return Page();
    }
}