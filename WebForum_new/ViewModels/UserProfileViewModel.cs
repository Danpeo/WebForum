using WebForum_new.Models;

namespace WebForum_new.ViewModels;

public class UserProfileViewModel
{
    public string UserName { get; set; }
    public string? UserBio { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? LastLogin { get; set; }
    public List<Models.Community>? Communities { get; set; }
    public List<Post>? Posts { get; set; }

    public UserProfileViewModel()
    {
        
    }
    
    public UserProfileViewModel(string userName, string? userBio, DateTime? registrationDate, DateTime? lastLogin, List<Models.Community>? communities, List<Post>? posts)
    {
        UserName = userName;
        UserBio = userBio;
        RegistrationDate = registrationDate;
        LastLogin = lastLogin;
        Communities = communities;
        Posts = posts;
    }
    
}