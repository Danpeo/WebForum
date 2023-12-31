using Microsoft.AspNetCore.Identity;

namespace WebForum_new.Models;

public class AppUser : IdentityUser
{
    public string? UserBio { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? LastLogin { get; set; }
    //public UserStatus Status { get; set; }
    public List<Community>? Communities { get; set; }
    public List<CommunitySubscription>? CommunitySubscriptions { get; set; }
    public List<Post>? Posts { get; set; } = new();
    public List<PostVote>? PostVotes { get; set; } = new();
    public List<Comment>? Comments { get; set; }
}