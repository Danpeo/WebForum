using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels.Post;

namespace WebForum_new.Services;

public interface IPostService
{
    Task<Post?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Community community, CreatePostViewModel? postVM, AppUser? createdBy);
    Task<List<ViewPostViewModel>> GetAllAsync();
}

public class PostService : CommonService<ApplicationDbContext>, IPostService
{
    public PostService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<ViewPostViewModel>> GetAllAsync()
    {
        List<ViewPostViewModel> posts = await Context.Posts
            .Include(c => c.Comments)
            .Select(c => new ViewPostViewModel()
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                DateTimeCreated = c.DateTimeCreated,
                Comments = c.Comments
            })
            .ToListAsync();

        return posts;
    }

    
    public async Task<Post?> GetByIdAsync(int id) =>
        await Context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> CreateAsync(Community community, CreatePostViewModel? postVM, AppUser? createdBy)
    {
        var newPost = new Post()
        {
            Title = postVM.Title,
            Content = postVM.Content,
            DateTimeCreated = postVM.DateTimeCreated,
            CreatedBy = createdBy
        };
        
        createdBy.Posts.Add(newPost);
        community.Posts?.Add(newPost);
        return await SaveAsync();
    }
}