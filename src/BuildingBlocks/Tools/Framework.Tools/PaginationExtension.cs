namespace Framework.Tools;

public static class PaginationExtension
{
    public static IQueryable<T> ToPaging<T>(this IOrderedQueryable<T> query, Paging paging) =>
        query.Skip(paging.PageSize * paging.PageIndex).Take(paging.PageSize);
}
