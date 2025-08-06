namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpdateHistoryResponseDto
{
    DateTime UpdatedDateTime { get; }
    UserBasicResponseDto UpdatedUser { get; }
    string UpdatedReason { get; }
}