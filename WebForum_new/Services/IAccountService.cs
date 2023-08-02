using WebForum_new.ViewModels;

namespace WebForum_new.Services;

public interface IAccountService
{
    Task<bool> Register(RegisterViewModel model);
    Task<bool> Login(LoginViewModel model);
    void Logout();
}