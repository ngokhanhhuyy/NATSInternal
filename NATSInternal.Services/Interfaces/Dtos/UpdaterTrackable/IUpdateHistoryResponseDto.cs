namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpdateHistoryResponseDto
{
    DateTime UpdatedDateTime { get; set; }
    UserBasicResponseDto UpdatedUser { get; set; }
    string Reason { get; set; }
}