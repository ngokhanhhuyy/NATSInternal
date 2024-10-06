namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpdateHistoryResponseDto
{
    DateTime UpdatedDateTime { get; }
    UserBasicResponseDto UpdatedUser { get; }
    string Reason { get; }
}