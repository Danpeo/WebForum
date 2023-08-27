using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels.Post;
using X.PagedList;

namespace WebForum_new.Services;

public class PostService : CommonService<ApplicationDbContext>, IPostService
{
    private readonly IImageService _imageService;
    private readonly ICommunityService _communityService;

    public PostService(ApplicationDbContext context, IImageService imageService, ICommunityService communityService) :
        base(context)
    {
        _imageService = imageService;
        _communityService = communityService;
    }

    public async Task<List<ViewPostViewModel>> GetAllAsync()
    {
        List<ViewPostViewModel> posts = await Context.Posts
            .Include(c => c.AppUser)
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

    public async Task<List<Post>> GetPostsFromSubscribedCommunitiesAsync(AppUser user)
    {
        List<int> subscribedCommunitiedIds = await _communityService.GetSubscribedCommunityIdsAsync(user);

        List<Post> posts = await Context.Communities
            .Where(c => subscribedCommunitiedIds.Contains(c.Id))
            .SelectMany(c => c.Posts)
            .OrderByDescending(p => p.LikeCount)
            .ThenByDescending(p => p.DateTimeCreated)
            .ToListAsync();

        return posts;
    }

    public IQueryable<Post> GetPostsQuery()
    {
        // В этом методе вы можете создать базовый запрос, который будет включать в себя
        // все необходимые Include, OrderBy и другие операции, которые вы хотите выполнить над данными.
        // Это позволит вам создать IQueryable<Post>, который будет содержать
        // только посты, соответствующие вашим требованиям.

        // Пример:
        var query = Context.Posts
            .Include(p => p.Comments)
            .Include(p => p.AppUser)
            .OrderByDescending(p => p.DateTimeCreated);

        return query;
    }

    
    public async Task<IPagedList<Post>> GetPostsFromSubscribedCommunitiesAsync(AppUser user, int pageNumber,  int pageSize)
    {
        List<Post> posts = await GetPostsFromSubscribedCommunitiesAsync(user);

        IPagedList<Post>? pagedPosts = posts.ToPagedList(pageNumber, pageSize);
        
        return pagedPosts;
    }

    public async Task<Post?> GetByIdAsync(int id) =>
        await Context.Posts
            .Include(p => p.Comments)
            .ThenInclude(c => c.AppUser)
            .Include(p => p.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> CreateAsync(Community community, CreatePostViewModel? postVM, AppUser? createdBy)
    {
        ImageUploadResult? image = await _imageService.UploadImageAsync(postVM?.Image);

        var newPost = new Post()
        {
            Title = postVM.Title,
            Content = postVM.Content,
            DateTimeCreated = postVM.DateTimeCreated,
            Image = image == null ? string.Empty : image.Url.ToString(),
            AppUser = createdBy
        };

        createdBy.Posts.Add(newPost);
        community.Posts?.Add(newPost);
        return await SaveAsync();
    }

    public async Task<bool> AddVoteAsync(int postId, AppUser user, VoteType voteType)
    {
        var vote = new PostVote()
        {
            PostId = postId,
            AppUser = user,
            VoteType = voteType
        };

        Post? post = await GetByIdAsync(postId);

        if (post == null)
            return false;

        if (voteType == VoteType.Like)
        {
            post.LikeCount++;
        }
        else if (voteType == VoteType.Dislike)
        {
            post.DislikeCount++;
        }

        user.PostVotes?.Add(vote);
        Context.PostVotes.Add(vote);

        return await SaveAsync();
    }

    public async Task<bool> RemoveVoteAsync(int postId, AppUser user, VoteType voteType)
    {
        PostVote? vote = await Context.PostVotes
            .FirstOrDefaultAsync(v => v.PostId == postId && v.AppUser == user && v.VoteType == voteType);

        Post? post = await GetByIdAsync(postId);

        if (post == null)
            return false;

        if (vote != null)
        {
            if (voteType == VoteType.Like)
            {
                post.LikeCount--;
            }
            else if (voteType == VoteType.Dislike)
            {
                post.DislikeCount--;
            }

            user.PostVotes?.Remove(vote);
            Context.PostVotes.Remove(vote);
        }

        return await SaveAsync();
    }
}