namespace HSX.Contract.Common;

public class PaginatedResponse<TValue> where TValue : class
{
    public IEnumerable<TValue> Items { get; private set; }
    public PaginationMetadata Pagination { get; private set; }

    public PaginatedResponse(HttpResponseMessage response, IEnumerable<TValue> items)
    {
        Items = items;
        Pagination = PaginationMetadata.FromHttpResponse(response);
    }
}
