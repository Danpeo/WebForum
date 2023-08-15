namespace WebForum_new.ViewModels.Post;

public class CreatePostViewModel
{
    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public CreatePostViewModel()
    {
        DateTimeCreated = DateTime.Now;
    }

    public CreatePostViewModel(string title, string content) : this()
    {
        Title = title;
        Content = content;
    }
}