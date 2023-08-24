using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Models;
using WebForum_new.ViewModels.Post;

namespace WebForum_new.Services;

public class PostService : CommonService<ApplicationDbContext>, IPostService
{
    private readonly IImageService _imageService;

    public PostService(ApplicationDbContext context, IImageService imageService) : base(context)
    {
        _imageService = imageService;
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
        var postVote = new PostVote()
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

        user.PostVotes?.Add(postVote);
        Context.PostVotes.Add(postVote);

        return await SaveAsync();
    }
}