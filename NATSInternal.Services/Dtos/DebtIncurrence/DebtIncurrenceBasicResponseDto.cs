﻿namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceBasicResponseDto
    : ICustomerEngageableBasicResponseDto<DebtIncurrenceAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long AmountAfterVat { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public DebtIncurrenceAuthorizationResponseDto Authorization { get; set; }

    internal DebtIncurrenceBasicResponseDto(DebtIncurrence debt)
    {
        MapFromEntity(debt);
    }

    internal DebtIncurrenceBasicResponseDto(
            DebtIncurrence debt,
            DebtIncurrenceAuthorizationResponseDto authorization)
    {
        MapFromEntity(debt);
        Authorization = authorization;
    }

    private void MapFromEntity(DebtIncurrence debt)
    {
        Id = debt.Id;
        AmountAfterVat = debt.Amount;
        Note = debt.Note;
        StatsDateTime = debt.StatsDateTime;
        IsLocked = debt.IsLocked;
        Customer = new CustomerBasicResponseDto(debt.Customer);
    }
}
