﻿namespace NATSInternal.Services.Dtos;

public class SupplyBasicResponseDto
{
    public int Id { get; set; }
    public DateTime PaidDateTime { get; set; }
    public long TotalAmount { get; set; }
    public bool IsLocked { get; set; }
    public UserBasicResponseDto User { get; set; }
    public string FirstPhotoUrl { get; set; }
    public SupplyAuthorizationResponseDto Authorization { get; set; }

    internal SupplyBasicResponseDto(Supply supply)
    {
        MapFromEntity(supply);
    }

    internal SupplyBasicResponseDto(Supply supply, SupplyAuthorizationResponseDto authorization)
    {
        MapFromEntity(supply);
        Authorization = authorization;
    }

    private void MapFromEntity(Supply supply)
    {
        Id = supply.Id;
        PaidDateTime = supply.SuppliedDateTime;
        TotalAmount = supply.TotalAmount;
        IsLocked = supply.IsLocked;
        User = new UserBasicResponseDto(supply.CreatedUser);
        FirstPhotoUrl = supply.FirstPhotoUrl;
    }
}
