namespace NATSInternal.Application.Extensions;

public static class DateOnlyExtensions
{
    #region Methods
    public static string ToVietnameseString(this DateOnly date)
    {
        return $"{date.Day} tháng {date.Month}, {date.Year}";
    }
    #endregion
}