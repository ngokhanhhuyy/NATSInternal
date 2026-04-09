using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Photos;

[UsedImplicitly]
internal class PhotoGetMutlipleByProductIdsValidator : Validator<PhotoGetMultipleByProductIdsRequestDto>
{
    #region Constructors
    public PhotoGetMutlipleByProductIdsValidator()
    {
        RuleFor(dto => dto.ProductIds)
            .NotEmpty()
            .WithName($"Danh sách {DisplayNames.ProductId.ToLower()}");
    }
    #endregion
}