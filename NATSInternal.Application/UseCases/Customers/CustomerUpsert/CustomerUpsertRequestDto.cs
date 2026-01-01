using NATSInternal.Application.Extensions;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerUpsertRequestDto : IRequestDto
{
    #region Properties
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? NickName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ZaloNumber { get; set; }
    public string? FacebookUrl { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public Guid? IntroducerId { get; set; }
    #endregion

    #region Methods
    public virtual void TransformValues()
    {
        MiddleName = MiddleName?.ToNullIfEmptyOrWhiteSpace();
        NickName = NickName?.ToNullIfEmptyOrWhiteSpace();
        PhoneNumber = PhoneNumber?.ToNullIfEmptyOrWhiteSpace();
        ZaloNumber = ZaloNumber?.ToNullIfEmptyOrWhiteSpace();
        FacebookUrl = FacebookUrl?.ToNullIfEmptyOrWhiteSpace();
        Email = Email?.ToNullIfEmptyOrWhiteSpace();
        Address = Address?.ToNullIfEmptyOrWhiteSpace();
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
        IntroducerId = IntroducerId == Guid.Empty ? null : IntroducerId;
    }
    #endregion
}