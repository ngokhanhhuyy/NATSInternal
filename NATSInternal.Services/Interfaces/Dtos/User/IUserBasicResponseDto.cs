namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUserBasicResponseDto<TAuthorization>
    :
        IUpsertableBasicResponseDto<TAuthorization>,
        IHasPhotoBasicResponseDto
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    string UserName { get; set; }
    string FirstName { get; set; }
    string MiddleName { get; set; }
    string LastName { get; set; }
    string FullName { get; set; }
    Gender Gender { get; set; }
    DateOnly? Birthday { get; set; }
    DateOnly? JoiningDate { get; set; }
    string AvatarUrl { get; set; }
}