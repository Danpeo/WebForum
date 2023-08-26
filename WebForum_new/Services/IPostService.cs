using WebForum_new.Models;
using WebForum_new.ViewModels.Post;
using X.PagedList;

namespace WebForum_new.Services;

public interface IPostService
{
    Task<Post?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Community community, CreatePostViewModel? postVM, AppUser? createdBy);
    Task<List<ViewPostViewModel>> GetAllAsync();
    Task<bool> AddVoteAsync(int postId, AppUser user, VoteType voteType);
    Task<bool> RemoveVoteAsync(int postId, AppUser user, VoteType voteType);
    Task<List<Post>> GetPostsFromSubscribedCommunitiesAsync(AppUser user);
    Task<IPagedList<Post>> GetPostsFromSubscribedCommunitiesAsync(AppUser user, int pageNumber, int pageSize);
}