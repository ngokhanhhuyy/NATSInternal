﻿namespace NATSInternal.Services.Dtos;

public class SupplyListResponseDto : IFinancialEngageableListResponseDto<
        SupplyBasicResponseDto,
        SupplyAuthorizationResponseDto,
        SupplyListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<SupplyBasicResponseDto> Items { get; set; }
    public List<MonthYearResponseDto> MonthYearOptions { get; set; }
    public SupplyListAuthorizationResponseDto Authorization { get; set; }
}