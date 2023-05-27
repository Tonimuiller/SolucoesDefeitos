namespace SolucoesDefeitos.Pesentation.RazorPages.Components.Pager.Model;

public sealed class PagerModel
{
    private int _pageSize = 10;
    private readonly int _pagingGroupSize = 3;

    public int TotalRecords { get; set; }
    public int TotalPages
    {
        get => (int)Math.Ceiling((decimal)TotalRecords / PageSize);
    }
    public int CurrentPage { get; set; }
    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSize = (value > 0) ? value : 10;
        }
    }
    public bool SplitPaging => Math.Floor(TotalPages / (decimal)PagingGroupSize) >= 3;
    public int PagingGroupSize
    {
        get => _pagingGroupSize;
    }
    public bool CurrentPageIsNextBottomPagingGroup => CurrentPage == (PagingGroupSize + 1);
    public bool CurrentPageIsNextUpperPagingGroup => CurrentPage == (TotalPages - (PagingGroupSize + 1));
    public List<(string, string)> AdditionalParameters { get; set; } = new List<(string, string)>();
    public string Path { get; set; } = string.Empty;
}
