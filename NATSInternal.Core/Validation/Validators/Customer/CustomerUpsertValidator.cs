namespace NATSInternal.Core.Validation.Validators;

internal class CustomerUpsertValidator : Validator<CustomerUpsertRequestDto>
{
    #region Constructors
    public CustomerUpsertValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotNull()
            .MaximumLength(CustomerContracts.FirstNameMaxLength)
            .WithName(dto => DisplayNames.FirstName);
        RuleFor(dto => dto.MiddleName)
            .MaximumLength(CustomerContracts.MiddleNameMaxLength)
            .WithName(dto => DisplayNames.MiddleName);
        RuleFor(dto => dto.LastName)
            .NotNull()
            .MaximumLength(CustomerContracts.LastNameMaxLength)
            .WithName(dto => DisplayNames.LastName);
        RuleFor(dto => dto.NickName)
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
    protected virtual bool IsValidFacebookUrl(string url)
    {
        if (url == null)
        {
            return true;
        }

        return url.StartsWith("https://facebook.com/");
    }
    #endregion
}