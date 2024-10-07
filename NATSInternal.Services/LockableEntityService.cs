namespace NATSInternal.Services;

public class LockableEntityService
{
    protected static List<MonthYearResponseDto> GenerateMonthYearOptions(
            MonthYearResponseDto earliestRecordedMonthYear)
    {
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        int currentYear = currentDateTime.Year;
        int currentMonth = currentDateTime.Month;
        List<MonthYearResponseDto> monthYearOptions = new List<MonthYearResponseDto>();
        if (earliestRecordedMonthYear != null)
        {
            for (int initializingYear = earliestRecordedMonthYear.Year;
                initializingYear <= currentYear;
                initializingYear++)
            {
                int initializingMonth = 1;
                if (initializingYear == earliestRecordedMonthYear.Year)
                {
                    initializingMonth = earliestRecordedMonthYear.Month;
                }
                
                while (initializingMonth <= 12)
                {
                    MonthYearResponseDto option;
                    option = new MonthYearResponseDto(
                        initializingYear,
                        initializingMonth);
                    monthYearOptions.Add(option);
                    initializingMonth++;
                    if (initializingYear == currentYear && initializingMonth > currentMonth)
                    {
                        break;
                    }
                }
            }
            monthYearOptions.Reverse();
        }
        else
        {
            monthYearOptions.Add(new MonthYearResponseDto(currentYear, currentMonth));
        }

        return monthYearOptions;
    }
}