using WebForum_new.Models;
using WebForum_new.ViewModels.Post;

namespace WebForum_new.Services;

public interface IPostService
{
    Task<Post?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Community community, CreatePostViewModel? postVM, AppUser? createdBy);
    Task<List<ViewPostViewModel>> GetAllAsync();
}