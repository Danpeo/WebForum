using System.ComponentModel.DataAnnotations;

namespace WebForum_new.ViewModels;

public class RegisterViewModel
{
    [Required] public string UserName { get; set; }

    [Required] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match!")]
    public string ConfirmPassword { get; set; }

    public RegisterViewModel()
    {
    }

    public RegisterViewModel(string email, string password, string confirmPassword, string userName)
    {
        Email = email;
        Password = password;
        //ConfirmPassword = confirmPassword;
        UserName = userName;
    }
}