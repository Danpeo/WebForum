using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Models;

namespace WebForum_new.Data;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Community?> Communities { get; set; }

    public DbSet<CommunitySubscription> CommunitySubscriptions { get; set; }

    // public DbSet<CommunityCreator> CommunityCreators { get; set; }
    // public DbSet<CommunityModerator> CommunityModerators { get; set; }
    public DbSet<Post> Posts { get; set; }

    /*public DbSet<PostVote> PostVotes { get; set; }
    public DbSet<CommentVote> CommentVotes { get; set; }*/
    public DbSet<AppUser> AppUsers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }
}