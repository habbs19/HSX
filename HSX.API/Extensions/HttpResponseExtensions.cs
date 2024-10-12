using HSX.Contract.Common;
using System.Text.Json;

namespace aznV.API.Extensions;

public static class HttpResponseExtensions
{
    public static void AddPagination(this HttpResponse response, PaginationMetadata metadata)
    {
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        response.Headers.Append("Pagination", JsonSerializer.Serialize(metadata, jsonOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");

        response.Headers.Append("X-Total-Count", metadata.TotalCount.ToString());
        response.Headers.Append("X-Per-Page", metadata.PageSize.ToString());
        response.Headers.Append("X-Current-Page", metadata.CurrentPage.ToString());
        response.Headers.Append("X-Total-Pages", metadata.TotalPages.ToString());
    }
}
