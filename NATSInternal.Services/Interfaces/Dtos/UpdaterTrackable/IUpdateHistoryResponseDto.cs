namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpdateHistoryResponseDto
{
    DateTime UpdatedDateTime { get; set; }
    UserBasicResponseDto UpdatedUser { get; set; }
    string Reason { get; set; }
}