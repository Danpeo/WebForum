using Microsoft.EntityFrameworkCore;

namespace WebForum_new.Services;

public abstract class CommonService<TDbContext> where TDbContext : DbContext
{
    protected readonly TDbContext _context;
    
    
    protected CommonService(TDbContext context)
    {
        _context = context;
    }
    
    protected virtual async Task<bool> SaveAsync()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }
}