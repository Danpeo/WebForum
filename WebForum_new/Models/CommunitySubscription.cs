namespace WebForum_new.Models;

public class CommunitySubscription
{
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public AppUser AppUser { get; set; }
}