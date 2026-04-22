using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductCategoryUpsertValidator<TRequestDto> : Validator<TRequestDto>
    where TRequestDto : ProductCategoryUpsertRequestDto
{
    #region Constructors
    public ProductCategoryUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(ProductCategoryContracts.NameMaxLength)
            .WithName(DisplayNames.Name);
    }
    #endregion
}
