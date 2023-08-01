using System.ComponentModel.DataAnnotations;

namespace WebForum_new.ViewModels;

public class LoginViewModel
{
    [Required] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public LoginViewModel()
    {
        
    }
    
    public LoginViewModel(string email, string password)
    {
        Email = email;
        Password = password;
    }
}