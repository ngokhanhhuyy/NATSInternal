namespace NATSInternal.Blazor.Models;

public class PaginationRangeModel
{
    public int Page { get; private set; }
    public int PageCount { get; private set; }
    public int LargeScreenStartingPage { get; private set; }
    public int LargeScreenEndingPage { get; private set; }
    public int SmallScreenStartingPage { get; private set; }
    public int SmallScreenEndingPage { get; private set; }

    public PaginationRangeModel(int page, int pageCount)
    {
        Page = page;
        PageCount = pageCount;
        (LargeScreenStartingPage, LargeScreenEndingPage) = CalculateRange(5);
        (SmallScreenStartingPage, SmallScreenEndingPage) = CalculateRange(3);
    }
    
    private (int, int) CalculateRange(int visibleButtonCount)
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
                startingPage = (int)Math.Ceiling(Page - (double)visibleButtonCount / 2);
                endingPage = (int)Math.Floor(Page + (double)visibleButtonCount / 2);
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