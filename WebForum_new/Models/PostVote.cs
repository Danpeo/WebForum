namespace WebForum_new.Models;

public class PostVote
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public VoteType VoteType { get; set; }
    public AppUser AppUser { get; set; }
}