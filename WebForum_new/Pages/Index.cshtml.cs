using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Services;

namespace WebForum_new.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IAccountService _accountService;


    public IndexModel(ILogger<IndexModel> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await _accountService.Logout();
        return RedirectToPage();
    }
}