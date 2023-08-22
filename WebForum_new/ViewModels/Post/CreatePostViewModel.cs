namespace WebForum_new.ViewModels.Post;

public class CreatePostViewModel
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public IFormFile? Image { get; set; }


    public CreatePostViewModel()
    {
        DateTimeCreated = DateTime.Now;
    }

    public CreatePostViewModel(string title, string content, IFormFile? image) : this()
    {
        Title = title;
        Content = content;
        Image = image;
    }
}