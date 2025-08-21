namespace NATSInternal.Core.Validation.Validators.Brand;

internal class BrandUpsertValidator : Validator<BrandUpsertRequestDto>
{
    #region Constructors
    public BrandUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(BrandContracts.NameMaxLength)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Website)
            .MaximumLength(BrandContracts.WebsiteMaxLength)
            .WithName(DisplayNames.Website);
        RuleFor(dto => dto.SocialMediaUrl)
            .MaximumLength(BrandContracts.SocialMediaUrlMaxLength)
            .WithName(DisplayNames.SocialMediaUrl);
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(BrandContracts.PhoneNumberMaxLength)
            .Matches(PhoneNumberRegex).WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.PhoneNumber);
        RuleFor(dto => dto.Email)
            .MaximumLength(BrandContracts.EmailMaxLength)
            .Matches(EmailRegex).WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Email);
        RuleFor(dto => dto.Address)
            .MaximumLength(BrandContracts.AddressMaxLength)
            .WithName(DisplayNames.Address);
        RuleFor(dto => dto.Photos).ContainsNoOrOneThumbnail();

        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Photos).SetValidator(new PhotoValidator(), ruleSets: "Create");
        });

        RuleSet("CreateAndUpdate", () =>
        {
            RuleForEach(dto => dto.Photos).SetValidator(new PhotoValidator(), ruleSets: "CreateAndUpdate");
        });
    }
    #endregion
}
