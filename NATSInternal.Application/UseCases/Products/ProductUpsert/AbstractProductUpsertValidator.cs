using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Application.Validation.Rules;
using NATSInternal.Application.Validation.Validators;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal abstract class AbstractProductUpsertValidator<TRequestDto> : Validator<TRequestDto>
    where TRequestDto : AbstractProductUpsertRequestDto
{
    #region Constructors
    protected AbstractProductUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(ProductContracts.NameMaxLength)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Description)
            .MaximumLength(ProductContracts.DescriptionMaxLength)
            .WithName(DisplayNames.Description);
        RuleFor(dto => dto.Unit)
            .NotEmpty()
            .MaximumLength(ProductContracts.UnitMaxLength)
            .WithName(DisplayNames.Unit);
        RuleFor(dto => dto.DefaultAmountBeforeVatPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.DefaultAmountBeforeVatPerUnit);
        RuleFor(dto => dto.DefaultVatPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithName(DisplayNames.DefaultVatPercentage);
        RuleFor(dto => dto.Photos).ContainsNoOrOneThumbnail();

        RuleSet("CreateAndUpdate", () =>
        {
            RuleForEach(dto => dto.Photos)
                .SetValidator(new PhotoAddOrUpdateValidator(), ruleSets: "CreateAndUpdate");
        });
    }
    #endregion
}