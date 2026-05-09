using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Photos;

internal class PhotoUpsertValidator : Validator<PhotoUpsertRequestDto>
{
    #region Constructors
    public PhotoUpsertValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.File)
                .IsValidImage()
                .WithName(DisplayNames.File);
        });

        RuleSet("CreateAndUpdate", () =>
        {
            RuleFor(dto => dto.File)
                .NotNull()
                .When(dto => !dto.Id.HasValue)
                .IsValidImage()
                .WithName(DisplayNames.File);
        });
    }
    #endregion
}