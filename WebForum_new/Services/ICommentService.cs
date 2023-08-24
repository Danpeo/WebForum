using WebForum_new.Models;
using WebForum_new.ViewModels.Comment;

namespace WebForum_new.Services;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetCommentsForPostAsync(int postId);
    Task<bool> CreateAsync(Post post, CreateCommentViewModel? commentVM, AppUser createdBy);
    Task<Comment?> GetByIdAsync(int id);
}