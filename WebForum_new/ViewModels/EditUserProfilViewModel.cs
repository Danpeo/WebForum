using WebForum_new.Models;

namespace WebForum_new.ViewModels;

public class EditUserProfilViewModel
{
    public string? UserBio { get; set; }

    public EditUserProfilViewModel()
    {
        
    }
    
    public EditUserProfilViewModel(AppUser user)
    {
        UserBio = user?.UserBio;
    }
}