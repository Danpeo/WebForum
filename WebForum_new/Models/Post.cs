namespace WebForum_new.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public List<Comment>? Comments { get; set; } = new();
    public AppUser AppUser { get; set; }
    public string? Image { get; set; }
}