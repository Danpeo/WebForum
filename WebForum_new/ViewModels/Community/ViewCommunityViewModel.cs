using WebForum_new.Models;

namespace WebForum_new.ViewModels.Community;

public class ViewCommunityViewModel : CommunityVewModel
{
    public int Id { get; set; }
    public string? Image { get; set; }
    public List<Models.Post>? Posts { get; set; } = new();
    public AppUser CreatedBy { get; set; }

    public ViewCommunityViewModel()
    {
        
    }
    
    public ViewCommunityViewModel(string name, string image, string description, DateTime dateTimeCreated) : base(name,
        description, dateTimeCreated)
    {
        Image = image;
    }
}