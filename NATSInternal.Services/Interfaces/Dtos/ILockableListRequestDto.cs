namespace NATSInternal.Services.Interfaces.Dtos;

public interface ILockableListRequestDto<TRequestDto> : IListRequestDto<TRequestDto> {
    int Month { get; set; }
    int Year { get; set; }
}