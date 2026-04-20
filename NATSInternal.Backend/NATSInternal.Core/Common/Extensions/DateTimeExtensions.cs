namespace NATSInternal.Core.Common.Extensions;

public static class DateTimeExtensions
{
    #region ExtensionMethods
    extension(DateTime dateTime)
    {
        public string DeltaTextFromDateTime(DateTime pastDateTime)
        {
            if (pastDateTime > dateTime)
            {
                throw new ArgumentException(
                    "Value for the pastDateTime property cannot be later " +
                    "than current CreatedDateTime value."
                );
            }

            int secondsDifference = (int)(dateTime - pastDateTime).TotalSeconds;
            string deltaText = ConvertSecondNumberToText(secondsDifference);
            
            if (deltaText != "Vừa xong")
            {
                return deltaText + " trước";
            }
            
            return deltaText;
        }

        public string DeltaTextUntilDateTime(DateTime futureDateTime)
        {
            if (futureDateTime < dateTime)
            {
                throw new ArgumentException(
                    "Value for the futureDateTime property cannot be earlier" +
                    "than current CreatedDateTime value."
                );
            }

            int secondsDifference = (int)(futureDateTime - dateTime).TotalSeconds;
            string deltaText = ConvertSecondNumberToText(secondsDifference);
            if (deltaText != "Vừa xong") return deltaText + " nữa";

            return deltaText;
        }

        public double YearDifferenceFromDateTime(DateTime pastDateTime)
        {
            if (pastDateTime > dateTime)
            {
                throw new ArgumentException(
                    "Value for the pastDateTime property cannot be later than current CreatedDateTime value."
                );
            }

            double daysDifference = (dateTime - pastDateTime).TotalDays;
            return Math.Round(daysDifference / 365.25, 1);
        }

        public string ToVietnameseString()
        {
            return $"{dateTime.Hour:D2}g{dateTime.Minute:D2}, {dateTime.Day:D2} tháng {dateTime.Month:D2}, {dateTime.Year}";
        }
        
        public DateTime ToApplicationTime()
        {
            return dateTime.AddHours(7);
        }
    }
    #endregion
    
    #region PrivateMethods
    private static string ConvertSecondNumberToText(double secondNumber, bool withSign = false)
    {
        string text;
        // Less than a minute, show "Just now"
        if (secondNumber < TimeSpan.FromMinutes(1).TotalSeconds)
        {
            text = "Vừa xong";
        }
        // Less than an hour, show "minutes"
        else if (secondNumber < TimeSpan.FromHours(1).TotalSeconds)
        {
            text = (int)Math.Round(secondNumber / 60) + " phút";
        }
        // Less than a day, show "hours"
        else if (secondNumber < TimeSpan.FromDays(1).TotalSeconds)
        {
            text = (int)Math.Round(secondNumber / (60 * 60)) + " giờ";
        }
        // Less than a month (30 days), show "days"
        else if (secondNumber < TimeSpan.FromDays(30).TotalSeconds)
        {
            text = (int)Math.Round(secondNumber / (60 * 60 * 24)) + " ngày";
        }
        // Less than a year, show "months"
        else if (secondNumber < TimeSpan.FromDays(365).TotalSeconds)
        {
            text = (int)Math.Round(secondNumber / (60 * 60 * 24 * 30)) + " tháng";
        }
        // More than a year, show "years"
        else
        {
            text = (int)Math.Round(secondNumber / (60 * 60 * 24 * 365)) + " năm";
        }

        // Adding sign
        if (withSign && secondNumber < 0)
        {
            return $"- {text}";
        }

        return text;
    }
    #endregion
}