using WebForum_new.Models;

namespace WebForum_new.ViewModels.Community;

public class DetailCommunityViewModel : CommunityVewModel
{
    public List<Models.Post>? Posts { get; set; } = new();

}