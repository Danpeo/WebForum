namespace WebForum_new.Pagination;

public class PaginationInfo
{
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int NextPage => CurrentPage + 1;
    public int PreviousPage => CurrentPage - 1;
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public string? RouteName { get; set; } = "id";
}