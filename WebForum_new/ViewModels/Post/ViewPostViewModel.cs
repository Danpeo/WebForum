namespace WebForum_new.ViewModels.Post;

public class ViewPostViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public List<Models.Comment>? Comments { get; set; } = new();

    public ViewPostViewModel()
    {
        
    }
    
    public ViewPostViewModel(int id, string title, string content, DateTime dateTimeCreated)
    {
        Id = id;
        Title = title;
        Content = content;
        DateTimeCreated = dateTimeCreated;
    }
}