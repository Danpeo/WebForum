using Microsoft.EntityFrameworkCore;
using WebForum_new.ViewModels;

namespace WebForum_new.Services;

public abstract class CommonService<TDbContext> where TDbContext : DbContext
{
    protected readonly TDbContext Context;
    
    
    protected CommonService(TDbContext context)
    {
        Context = context;
    }
    
    protected virtual async Task<bool> SaveAsync()
    {
        var saved = await Context.SaveChangesAsync();
        return saved > 0;
    }
}