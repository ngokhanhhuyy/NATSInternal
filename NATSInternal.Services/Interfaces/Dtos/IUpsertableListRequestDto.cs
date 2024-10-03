namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableListRequestDto<TRequestDto> : IListRequestDto<TRequestDto>
{
    CreatedUserId
}