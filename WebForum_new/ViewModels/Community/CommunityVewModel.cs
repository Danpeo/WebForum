namespace WebForum_new.ViewModels.Community;

public abstract class CommunityVewModel
{
    public string Name { get; set; }

    public string Description { get; set; }
    
    public DateTime DateTimeCreated { get; set; }

    public CommunityVewModel()
    {
        
    }
    
    public CommunityVewModel(string name, string description, DateTime dateTimeCreated)
    {
        Name = name;
        Description = description;
        DateTimeCreated = dateTimeCreated;
    }
}