
namespace HSX.Contract.Common;
public class PaginationParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int CurrentPage { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
 }