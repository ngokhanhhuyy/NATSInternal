using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Web.Extensions;

public static class IHtmlHelperExtensions
{
    #region ExtensionMethods
    public static IHtmlContent DisplayGender<TModel>(this IHtmlHelper<TModel> _, Gender gender)
    {
        return new HtmlString(gender == Gender.Male ? "Nam" : "Nữ");
    }
    
    public static IHtmlContent DisplayPhoneNumber<TModel>(this IHtmlHelper<TModel> _, string? phoneNumber)
    {
        if (phoneNumber is null)
        {
            return HtmlString.Empty;
        }

        if (phoneNumber.Length == 10)
        {
            return new HtmlString($"{phoneNumber[..4]}-{phoneNumber[4..7]}-{phoneNumber[7..]}");
        }

        return new HtmlString(phoneNumber);
    }
    
    public static IHtmlContent DisplayDateOnly<TModel>(this IHtmlHelper<TModel> _, DateOnly? date)
    {
        if (!date.HasValue)
        {
            return HtmlString.Empty;
        }

        return new HtmlString($"Ngày {date.Value.Day} tháng {date.Value.Month}, năm {date.Value.Year}");
    }
    
    public static IHtmlContent DisplayDateTime<TModel>(this IHtmlHelper<TModel> _, DateTime? dateTime)
    {
        if (!dateTime.HasValue)
        {
            return HtmlString.Empty;
        }

        int day = dateTime.Value.Day;
        int month = dateTime.Value.Month;
        int year = dateTime.Value.Year;
        int hour = dateTime.Value.Hour;
        int minute = dateTime.Value.Minute;
        return new HtmlString($"{hour} giờ {minute}, ngày {day} tháng {month}, năm {year}");
    }
    
    public static IHtmlContent DisplayCurrency<TModel>(this IHtmlHelper<TModel> html, long? amount)
    {
        if (!amount.HasValue)
        {
            return HtmlString.Empty;
        }

        return new HtmlString($"{amount:N0}đ");
    }

    public static string GetDefaultImageUrl(this IHtmlHelper _)
    {
        return "/output/images/default.jpg";
    }
    #endregion
}
