using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Services;

namespace WebForum_new.Pages.Community;

public class DetailModel : PageModel
{
    public Models.Community? Community { get; set; } = new();
    
    private readonly ICommunityService _communityService;

    public DetailModel(ICommunityService communityService)
    {
        _communityService = communityService;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        Community = await _communityService.GetByIdAsync(id);

        if (Community == null)
            return NotFound();

        return Page();
    }
}