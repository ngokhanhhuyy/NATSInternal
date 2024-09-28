namespace NATSInternal.Models;

public class ListModel<TModel>
{
    public virtual string OrderByField { get; set; }
    public virtual bool OrderByAscending { get; set; }
    public virtual int Page { get; set; }
    public virtual int ResultsPerPage { get; set; } = 15;
    public virtual int PageCount { get; set; }
    public List<TModel> Items { get; set; }

    public int LargeScreenStartingPage { get; init; }
    public int LargeScreenEndingPage { get; init; }
    public int SmallScreenStartingPage { get; init; }
    public int SmallScreenEndingPage { get; init; }

    protected ListModel()
    {
        (LargeScreenStartingPage, LargeScreenEndingPage) = CalculatePaginationRange(5);
        (SmallScreenStartingPage, SmallScreenEndingPage) = CalculatePaginationRange(3);
    }

    private (int, int) CalculatePaginationRange(int visibleButtonCount)
    {
        int startingPage;
        int endingPage;
        
        if (PageCount >= visibleButtonCount)
        {
            if (Page - (int)Math.Floor((double)visibleButtonCount / 2) <= 1)
            {
                startingPage = 1;
                endingPage = startingPage + (visibleButtonCount - 1);
            }
            else if (Page + (int)Math.Floor((double)visibleButtonCount / 2) > PageCount)
            {
                endingPage = PageCount;
                startingPage = endingPage - (visibleButtonCount - 1);
            }
            else
            {
                startingPage = (int)Math.Ceiling((double)Page - visibleButtonCount / 2);
                endingPage = (int)Math.Floor((double)Page + visibleButtonCount / 2);
            }
        }
        else
        {
            startingPage = 1;
            endingPage = Page;
        }

        return (startingPage, endingPage);
    }
}