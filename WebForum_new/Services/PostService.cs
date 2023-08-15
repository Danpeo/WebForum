using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels.Post;

namespace WebForum_new.Services;

public interface IPostService
{
    Task<Post?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Community community, CreatePostViewModel? postVM);
}

public class PostService : CommonService<ApplicationDbContext>, IPostService
{
    public PostService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Post?> GetByIdAsync(int id) =>
        await Context.Posts
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> CreateAsync(Community community, CreatePostViewModel? postVM)
    {
        var newPost = new Post()
        {
            Title = postVM.Title,
            Content = postVM.Content,
            DateTimeCreated = postVM.DateTimeCreated
        };
            
        community.Posts?.Add(newPost);
        return await SaveAsync();
    }
}