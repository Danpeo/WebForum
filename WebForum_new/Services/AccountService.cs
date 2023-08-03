using Microsoft.AspNetCore.Identity;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels;

namespace WebForum_new.Services;

public class AccountService : CommonService<ApplicationDbContext>, IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountService(ApplicationDbContext context, UserManager<AppUser> userManager) : base(context)
    {
        _userManager = userManager;
    }
    
    public AccountService(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : this(context, userManager)
    {
        _signInManager = signInManager;
    }

    public async Task<bool> Register(RegisterViewModel model)
    {
        AppUser user = await _userManager.FindByEmailAsync(model.Email);

        if (user != null)
            return false;

        var newUser = new AppUser()
        {
            UserName = model.UserName,
            Email = model.Email,
            RegistrationDate = DateTime.Now,
            LastLogin = DateTime.Now,
        };

        IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
        return result.Succeeded;
    }

    public async Task<bool> Login(LoginViewModel model)
    {
        AppUser user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return false;

        bool passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!passwordCheck)
            return false;
        
        SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        return result.Succeeded;
    }

    public async Task Logout() => 
        await _signInManager.SignOutAsync();
}