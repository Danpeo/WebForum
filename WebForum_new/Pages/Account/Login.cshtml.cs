using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebForum_new.Data;
using WebForum_new.Filters;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.ViewModels;

namespace WebForum_new.Pages.Account;

[ValidateModel]
public class LoginModel : PageModel
{
    [BindProperty] public LoginViewModel? LoginVM { get; set; } = new();

    private readonly IAccountService _accountService;

    public LoginModel(ApplicationDbContext context, UserManager<AppUser> userManager, IAccountService accountService)
    {
        _accountService = accountService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        bool result = LoginVM != null && await _accountService.Login(LoginVM);
        if (result)
            return RedirectToPage("/Index");

        return Page();
    }
}