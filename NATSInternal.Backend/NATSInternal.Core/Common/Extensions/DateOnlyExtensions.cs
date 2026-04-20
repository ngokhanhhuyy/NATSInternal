namespace NATSInternal.Core.Common.Extensions;

public static class DateOnlyExtensions
{
    #region Methods
    extension(DateOnly date)
    {
        public string ToVietnameseString()
        {
            return $"{date.Day} tháng {date.Month}, {date.Year}";
        }
    }
    #endregion
}