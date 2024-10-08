namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpdateHistoryResponseDto<TUserBasic, TUserAuthorization>
    where TUserBasic : IUserBasicResponseDto<TUserAuthorization>
    where TUserAuthorization : IUpsertableAuthorizationResponseDto
{
    DateTime UpdatedDateTime { get; set; }
    TUserBasic UpdatedUser { get; set; }
    string Reason { get; set; }
}