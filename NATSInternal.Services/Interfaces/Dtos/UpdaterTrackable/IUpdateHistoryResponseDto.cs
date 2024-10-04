namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpdateHistoryResponseDto
{
    DateTime UpdatedDateTime { get; internal set; }
    UserBasicResponseDto UpdatedUser { get; internal set; }
    string Reason { get; internal set; }
}