using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Services;

namespace WebForum_new.Pages.Community.Post;

public class DetailModel : PageModel
{
    public Models.Post? Post { get; set; } = new();
    
    private readonly IPostService _postService;

    public DetailModel(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        Post = await _postService.GetByIdAsync(id);

        if (Post == null)
            return NotFound();

        return Page();
    }
}