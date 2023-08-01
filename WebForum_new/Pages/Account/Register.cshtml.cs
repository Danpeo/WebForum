using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels;

namespace WebForum_new.Pages.Account;

public class RegisterModel : PageModel
{
    [BindProperty] 
    public RegisterViewModel? RegisterVM { get; set; } = new();

    private readonly IAccountService _accountService;

    public RegisterModel(ApplicationDbContext context, UserManager<AppUser> userManager, IAccountService accountService)
    {
        _accountService = accountService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            bool result = RegisterVM != null && await _accountService.Register(RegisterVM);
            if (result)
                return RedirectToPage("/Index");
        }

        return Page();
    }
}