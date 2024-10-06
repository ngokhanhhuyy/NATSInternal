namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICustomerEntity<TCustomer, TUser> : IIdentifiableEntity<TCustomer>
    where TCustomer : class, new()
    where TUser : class, IUserEntity<TUser>, new()

{
    string FirstName { get; set; }
    string NormalizedFirstName { get; set; }
    string MiddleName { get; set; }
    string NormalizedMiddleName { get; set; }
    string LastName { get; set; }
    string NormalizedLastName { get; set; }
    string FullName { get; set; }
    string NormalizedFullName { get; set; }
    string NickName { get; set; }
    Gender Gender { get; set; }
    DateOnly? Birthday { get; set; }
    string PhoneNumber { get; set; }
    string ZaloNumber { get; set; }
    string FacebookUrl { get; set; }
    string Email { get; set; }
    string Address { get; set; }

    TUser CreatedUser { get; set; }
}
