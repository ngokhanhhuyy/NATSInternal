namespace NATSInternal.Core.Validation.Validators.Brand;

internal class BrandUpsertValidator : Validator<BrandUpsertRequestDto>
{
    public BrandUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Website)
            .MaximumLength(255)
            .WithName(DisplayNames.Website);
        RuleFor(dto => dto.SocialMediaUrl)
            .MaximumLength(1000)
            .WithName(DisplayNames.SocialMediaUrl);
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(15)
            .Matches(PhoneNumberRegex).WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.PhoneNumber);
        RuleFor(dto => dto.Email)
            .MaximumLength(255)
            .Matches(EmailRegex).WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Email);
        RuleFor(dto => dto.Address)
            .MaximumLength(255)
            .WithName(DisplayNames.Address);
        RuleFor(dto => dto.ThumbnailFile)
            .Must(IsValidImage)
            .When(dto => dto.ThumbnailFile != null)
            .WithName(DisplayNames.ThumbnailFile);
    }
}
