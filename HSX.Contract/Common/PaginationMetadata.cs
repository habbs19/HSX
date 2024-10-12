using System.Text.Json;

namespace HSX.Contract.Common;

public class PaginationMetadata
{
    private string _customMessage = string.Empty;

    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    /// <summary>
    /// The Message property, allowing for a custom message or default message
    /// </summary>
    public string Message
    {
        get => string.IsNullOrEmpty(_customMessage) ? $"Page {CurrentPage} of {TotalPages}" : _customMessage;
        set => _customMessage = value;  // Set a custom message
    }
    /// <summary>
    /// Method to reset the custom message and use the default
    /// </summary>
    public void ResetMessage()
    {
        _customMessage = string.Empty;
    }

    //public PaginationMetadata(HttpResponseMessage response)
    //{
    //    var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    //    response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));
    //    response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

    //    if (response.Headers.TryGetValues("X-Total-Count", out var total_items_header))
    //    {
    //        TotalCount = int.Parse(total_items_header.First());
    //    }

    //    if (response.Headers.TryGetValues("X-Per-Page", out var page_size_header))
    //    {
    //        PageSize = int.Parse(page_size_header.First());
    //    }

    //    if (response.Headers.TryGetValues("X-Current-Page", out var current_page_header))
    //    {
    //        CurrentPage = int.Parse(current_page_header.First());
    //    }
    //}
    public static PaginationMetadata FromHttpResponse(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues(Constants.PaginationHeader, out var pagination_header))
        {
            var pagination_json = pagination_header.First();
            return JsonSerializer.Deserialize<PaginationMetadata>(pagination_json)!;
        }

        return null!;
    }
}
