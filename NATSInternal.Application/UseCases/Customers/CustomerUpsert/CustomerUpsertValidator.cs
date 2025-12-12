using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Rules;
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
            .IsValidName()
            .WithName(DisplayNames.FirstName);
        RuleFor(dto => dto.MiddleName)
            .MinimumLength(1)
            .MaximumLength(CustomerContracts.MiddleNameMaxLength)
            .IsValidName()
            .WithName(DisplayNames.MiddleName);
        RuleFor(dto => dto.LastName)
            .NotEmpty()
            .MaximumLength(CustomerContracts.LastNameMaxLength)
            .IsValidName()
            .WithName(DisplayNames.LastName);
        RuleFor(dto => dto.NickName)
            .MinimumLength(1)
            .MaximumLength(CustomerContracts.NickNameMaxLength)
            .Matches(@"^\p{L}+$")
            .WithName(DisplayNames.NickName);
        RuleFor(dto => dto.Gender)
            .IsInEnum()
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Gender);
        RuleFor(dto => dto.Birthday)
            .Must(EqualOrEarlierThanToday)
            .WithName(DisplayNames.Birthday);
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(CustomerContracts.PhoneNumberMaxLength)
            .Matches(@"^[0-9]*$")
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.PhoneNumber);
        RuleFor(dto => dto.ZaloNumber)
            .MaximumLength(CustomerContracts.ZaloNumberMaxLength)
            .Matches(@"^[0-9]*$")
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.ZaloNumber);
        RuleFor(dto => dto.FacebookUrl)
            .Must(IsValidFacebookUrl)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.FacebookUrl);
        RuleFor(dto => dto.Email)
            .EmailAddress()
            .WithName(DisplayNames.Email);
        RuleFor(dto => dto.Address)
            .MaximumLength(CustomerContracts.AddressMaxLength)
            .WithName(DisplayNames.Address);
        RuleFor(dto => dto.Note)
            .MaximumLength(CustomerContracts.NoteMaxLength)
            .WithName(DisplayNames.Note);
    }
    #endregion

    #region ProtectedMethods
    protected bool IsValidFacebookUrl(string? url)
    {
        if (url == null)
        {
            return true;
        }

        return url.StartsWith("https://facebook.com/");
    }
    #endregion
}