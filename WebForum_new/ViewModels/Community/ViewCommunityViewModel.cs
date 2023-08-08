using WebForum_new.Models;

namespace WebForum_new.ViewModels.Community;

public class ViewCommunityViewModel : CommunityVewModel
{
    public List<Post>? Posts { get; set; } = new();
    
}