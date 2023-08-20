using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels.Comment;

namespace WebForum_new.Services;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetCommentsForPostAsync(int postId);
    Task<bool> CreateAsync(Post post, CreateCommentViewModel? commentVM, AppUser? createdBy);
    Task<Comment?> GetByIdAsync(int id);
}

public class CommentService : CommonService<ApplicationDbContext>, ICommentService
{
    public CommentService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Comment?> GetByIdAsync(int id) =>
        await Context.Comments
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id);
    
    public async Task<IEnumerable<Comment>> GetCommentsForPostAsync(int postId)
    {
        /*return await Context.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();*/
        return null;
    }

    public async Task<bool> CreateAsync(Post post, CreateCommentViewModel? commentVM, AppUser? createdBy)
    {
        var newComment = new Comment()
        {
            Content = commentVM.Content,
            DateTimeCreated = DateTime.Now,
            AppUser = createdBy
        };
        
        post.Comments.Add(newComment);
        return await SaveAsync();
    }
}