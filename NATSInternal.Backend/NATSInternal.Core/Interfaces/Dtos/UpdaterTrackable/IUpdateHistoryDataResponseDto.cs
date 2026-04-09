namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpdateHistoryDataResponseDto
{
    #region
    DateTime UpdatedDateTime { get; }
    UserBasicResponseDto UpdatedUser { get; }
    string UpdatedReason { get; }
    #endregion
}