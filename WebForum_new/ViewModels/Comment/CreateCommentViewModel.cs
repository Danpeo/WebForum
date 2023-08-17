namespace WebForum_new.ViewModels.Comment;

public class CreateCommentViewModel
{
    public string Content { get; set; }

    public DateTime DateTimeCreated { get; set; }

    public CreateCommentViewModel()
    {
        DateTimeCreated = DateTime.Now;
    }

    public CreateCommentViewModel(string content) : this()
    {
        Content = content;
    }
}