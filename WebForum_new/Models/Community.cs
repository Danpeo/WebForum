namespace WebForum_new.Models;

public class Community
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public List<Post>? Posts { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public AppUser AppUser { get; set; }
}