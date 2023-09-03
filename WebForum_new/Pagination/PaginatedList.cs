using WebForum_new.ViewModels.Community;

namespace WebForum_new.Pagination;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; }
    public int TotalPages { get; }

    public PaginatedList()
    {
    }
    
    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
    {
        IEnumerable<T> enumerable = source.ToList();
        IEnumerable<T> items = enumerable
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        
        int count = enumerable.Count();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}