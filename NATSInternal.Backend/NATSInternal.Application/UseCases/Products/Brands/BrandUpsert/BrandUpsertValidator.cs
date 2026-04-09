using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Rules;
using NATSInternal.Application.Validation.Validators;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandUpsertValidator<TRequestDto> : Validator<TRequestDto> where TRequestDto : BrandUpsertRequestDto
{
    #region Constructors
    public BrandUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MinimumLength(BrandContracts.NameMinLength)
            .MaximumLength(BrandContracts.NameMaxLength)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Website)
            .MaximumLength(BrandContracts.WebsiteMaxLength)
            .IsValidWebsiteUrl()
            .WithName(DisplayNames.Website);
        RuleFor(dto => dto.SocialMediaUrl)
            .MaximumLength(BrandContracts.SocialMediaUrlMaxLength)
            .IsValidWebsiteUrl()
            .WithName(DisplayNames.SocialMediaUrl);
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(BrandContracts.PhoneNumberMaxLength)
            .IsValidPhoneNumber()
            .WithName(DisplayNames.PhoneNumber);
        RuleFor(dto => dto.Email)
            .MaximumLength(BrandContracts.EmailMaxLength)
            .EmailAddress()
            .WithName(DisplayNames.Email);
        RuleFor(dto => dto.Address)
            .MaximumLength(BrandContracts.AddressMaxLength)
            .WithName(DisplayNames.Address);
    }
    #endregion

    #region Methods
    #endregion
}
