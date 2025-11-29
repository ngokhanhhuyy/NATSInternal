using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

internal class CustomerUpsertValidator<TRequestDto> : Validator<TRequestDto>
    where TRequestDto : CustomerUpsertRequestDto
{
    #region Constructors
    public CustomerUpsertValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty()
            .MaximumLength(CustomerContracts.FirstNameMaxLength)
            .WithName(dto => DisplayNames.FirstName);
        RuleFor(dto => dto.MiddleName)
            .MinimumLength(1)
            .MaximumLength(CustomerContracts.MiddleNameMaxLength)
            .WithName(dto => DisplayNames.MiddleName);
        RuleFor(dto => dto.LastName)
            .NotEmpty()
            .MaximumLength(CustomerContracts.LastNameMaxLength)
            .WithName(dto => DisplayNames.LastName);
        RuleFor(dto => dto.NickName)
            .MinimumLength(1)
            .MaximumLength(CustomerContracts.NickNameMaxLength)
            .WithName(dto => DisplayNames.NickName);
        RuleFor(dto => dto.Gender)
            .IsInEnum()
            .WithMessage(ErrorMessages.Invalid)
            .WithName(dto => DisplayNames.Gender);
        RuleFor(dto => dto.Birthday)
            .Must(EqualOrEarlierThanToday)
            .WithName(dto => DisplayNames.Birthday);
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(CustomerContracts.PhoneNumberMaxLength)
            .Matches(@"^[0-9]*$")
            .WithMessage(ErrorMessages.Invalid)
            .WithName(dto => DisplayNames.PhoneNumber);
        RuleFor(dto => dto.ZaloNumber)
            .MaximumLength(CustomerContracts.ZaloNumberMaxLength)
            .Matches(@"^[0-9]*$")
            .WithMessage(ErrorMessages.Invalid)
            .WithName(dto => DisplayNames.ZaloNumber);
        RuleFor(dto => dto.FacebookUrl)
            .Must(IsValidFacebookUrl)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(dto => DisplayNames.FacebookUrl);
        RuleFor(dto => dto.Email)
            .EmailAddress()
            .WithName(dto => DisplayNames.Email);
        RuleFor(dto => dto.Address)
            .MaximumLength(CustomerContracts.AddressMaxLength)
            .WithName(dto => DisplayNames.Address);
        RuleFor(dto => dto.Note)
            .MaximumLength(CustomerContracts.NoteMaxLength)
            .WithName(dto => DisplayNames.Note);
    }
    #endregion

    #region ProtectedMethods
    protected virtual bool IsValidFacebookUrl(string? url)
    {
        if (url == null)
        {
            return true;
        }

        return url.StartsWith("https://facebook.com/");
    }
    #endregion
}