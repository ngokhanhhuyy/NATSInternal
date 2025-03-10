﻿namespace NATSInternal.Services.Dtos;

public class BrandListResponseDto : IUpsertableListResponseDto<
        BrandBasicResponseDto,
        BrandExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<BrandBasicResponseDto> Items { get; set; }
}
