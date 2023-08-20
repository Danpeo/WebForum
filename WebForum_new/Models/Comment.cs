namespace WebForum_new.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public AppUser AppUser { get; set; }
}