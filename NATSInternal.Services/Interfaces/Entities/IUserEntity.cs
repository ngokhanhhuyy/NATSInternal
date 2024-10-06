namespace NATSInternal.Services.Interfaces.Entities;

internal interface IUserEntity<T> : IIdentifiableEntity<T> where T : class, new()
{
    string FirstName { get; set; }
    string NormalizedFirstName { get; set; }
    string MiddleName { get; set; }
    string NormalizedMiddleName { get; set; }
    string LastName { get; set; }
    string NormalizedLastName { get; set; }
    string FullName { get; set; }
    string NormalizedFullName { get; set; }
    Gender Gender { get; set; }
    DateOnly? Birthday { get; set; }
    DateOnly? JoiningDate { get; set; }
    DateTime CreatedDateTime { get; set; }
    DateTime? UpdatedDateTime { get; set; }
    string Note { get; set; }
    string AvatarUrl { get; set; }
    bool IsDeleted { get; set; }
    byte[] RowVersion { get; set; }

    bool HasPermission(string permissionName);
}