﻿namespace NATSInternal.Services.Dtos;

public class SupplyListRequestDto : IProductEngageableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; } = nameof(FieldOptions.PaidDateTime);
    public int Year { get; set; }
    public int Month { get; set; }
    public bool IgnoreMonthYear { get; set; }
    public int? CreatedUserId { get; set; }
    public int? ProductId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty();

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        if (!IgnoreMonthYear)
        {
            if (Month == 0)
            {
                Month = currentDateTime.Month;
            }

            if (Year == 0)
            {
                Year = currentDateTime.Year;
            }
        }
    }

    public enum FieldOptions
    {
        TotalAmount,
        PaidDateTime,
        ShipmentFee,
        ItemAmount,
    }
}