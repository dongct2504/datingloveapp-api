namespace DatingLoveApp.Business.Dtos;

public class PaginationHeader
{
    public PaginationHeader(
        int pageNumber,
        int pageSize,
        int totalRecords,
        int totalPages,
        bool hasNextPage,
        bool hasPreviousPage)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = totalPages;
        HasNextPage = hasNextPage;
        HasPreviousPage = hasPreviousPage;
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalRecords { get; }

    public int TotalPages { get; }

    public bool HasNextPage { get; }

    public bool HasPreviousPage { get; }
}
