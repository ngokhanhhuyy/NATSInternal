using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Customers;

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
    public int? IntroducerId { get; set; }
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

        if (IntroducerId == 0)
        {
            IntroducerId = null;
        }
    }
    #endregion
}