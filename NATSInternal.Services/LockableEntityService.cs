namespace NATSInternal.Services;

public class LockableEntityService
{
    protected static List<ListMonthYearResponseDto> GenerateMonthYearOptions(
            ListMonthYearResponseDto earliestRecordedMonthYear)
    {
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        int currentYear = currentDateTime.Year;
        int currentMonth = currentDateTime.Month;
        List<ListMonthYearResponseDto> monthYearOptions = new List<ListMonthYearResponseDto>();
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
                    ListMonthYearResponseDto option;
                    option = new ListMonthYearResponseDto(
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
            monthYearOptions.Add(new ListMonthYearResponseDto(currentYear, currentMonth));
        }

        return monthYearOptions;
    }
}